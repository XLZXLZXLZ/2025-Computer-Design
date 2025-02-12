using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.UI;

public class SingleDialogueController : MonoBehaviour
{
    [SerializeField] SingleDialogueConfig DialogueConfig;
    [SerializeField] List<Lines> Dialogue = new();
    [SerializeField] Image ActorIcon;
    [SerializeField] Text Words;
    [SerializeField] Text ActorName;
    [SerializeField] int currentLineIndex = 0;
    [SerializeField] VerticalLayoutGroup chioceHolder;
    [SerializeField] GameObject buttonPrefab;

    [Tooltip("打字机效果时间间隔")]
    [SerializeField] float typingSpeed;//打字机效果时间间隔
    [SerializeField] KeyCode nextLineKey = KeyCode.K;
    [SerializeField] KeyCode skipCurrentLineKey = KeyCode.L;

    Coroutine TypingWords;
    bool waitForButtonClick = false;

    private void OnEnable()
    {
        LoadDialogueConfig();
        currentLineIndex = 0;

        ShowNextLine(Dialogue[currentLineIndex]);
    }

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        if (Input.GetKeyDown(nextLineKey) && TypingWords == null && !waitForButtonClick)
        {
            PushDialogue();
        }
        else if (Input.GetKeyDown(skipCurrentLineKey) && TypingWords != null) {
            EndType();
        }
    }

    private void PushDialogue()
    {
        if (currentLineIndex == Dialogue.Count - 1)
        {
            EndDialogue();
            return;
        }
        ShowNextLine(Dialogue[++currentLineIndex]);
    }

    /// <summary>
    /// 显示下一行对话
    /// </summary>
    /// <param name="nextLine">下一行对话的数据</param>
    void ShowNextLine(Lines nextLine) {
        ActorIcon.sprite = DialogueConfig.Actors.GetIconWithName(nextLine.ActorName);
        ActorName.text = nextLine.ActorName;
        TypingWords = StartCoroutine(TypeWords(nextLine.Sentence));
        if (nextLine.Chioces.Count > 0) {
            waitForButtonClick = true;
            StartCoroutine(GenerateChioceButtom(nextLine.Chioces));
        }
    }

    IEnumerator GenerateChioceButtom(List<string> chioces) {
        yield return new WaitForFixedUpdate();
        while (TypingWords != null) {
            //Debug.Log("Waiting for typing...");
            yield return new WaitForEndOfFrame();
            //Debug.Log("After yield retrun");
        }
        //Debug.Log("Ready to show button");

        foreach (var chiocesText in chioces) {
            var newButtom = Instantiate(buttonPrefab);
            newButtom.GetComponentInChildren<Text>().text = chiocesText;
            newButtom.GetComponent<Button>().onClick.AddListener(() => OnChoiceButtonClicked());
            newButtom.transform.SetParent(chioceHolder.transform);
        }
        yield return null;
    }

    void OnChoiceButtonClicked() {
        GameObject button;

        for (int i = chioceHolder.transform.childCount-1;i >= 0; i--) {
            button = chioceHolder.transform.GetChild(i).gameObject;
            button.GetComponent<Button>().onClick.RemoveAllListeners();
            Destroy(button);
        }
        waitForButtonClick = false;
        PushDialogue();
    }

    IEnumerator TypeWords(string words) { 
        yield return new WaitForEndOfFrame();

        for (int i=0;i < words.Count(); i++) { 
            Words.text = words[..i];
            yield return new WaitForSeconds(typingSpeed);   
        }

        TypingWords = null;
    }

    void EndType() {
        StopCoroutine(TypingWords);
        TypingWords = null;
        Words.text = Dialogue[currentLineIndex].Sentence;
    }

    void LoadDialogueConfig() {
        Dialogue.Clear();

        string[] lines = DialogueConfig.Dialogue.text.Split(new[] { '\n', '\r' }, System.StringSplitOptions.RemoveEmptyEntries);
        for (int i=0;i < lines.Count(); i++)
        {
            string line = lines[i];
            if (line.StartsWith("@")) { 
                string ActorName = line.Substring(1);
                if (DialogueConfig.Actors.IfActorExistInConfig(ActorName))
                {
                    i++;
                    line = lines[i];
                    var newLine = new Lines(ActorName, line);
                    Dialogue.Add(newLine);
                    if (i < lines.Count() - 1 && lines[i + 1].StartsWith("#")) {
                        i++;
                        for (;i < lines.Count() && lines[i].StartsWith("#"); i++)
                        {
                            Debug.Log("检测到按钮");
                            newLine.Chioces.Add(lines[i].Substring(1));
                        }
                    }
                } else {
                    Debug.LogError("读取到不存在的演员信息");
                    EndDialogue();
                }
            }
        }
    }

    void EndDialogue() {
        //结束对话
        Dialogue.Clear();

        //关闭对话框

        gameObject.SetActive(false);
    }
}

public static class DialogueSystemExtension {
    public static bool IfActorExistInConfig(this List<SingleDialogueActorData> actors,string doubtfulActor) {
        foreach (var actor in actors) {
            if (actor.name.Equals(doubtfulActor)) return true;
        }

        return false;
    }

    public static Sprite GetIconWithName(this List<SingleDialogueActorData> actors,string actorName) {
        foreach (var actor in actors) { 
            if(actor.Name.Equals(actorName)) return actor.Icon;
        }
        Debug.LogError("未在列表中找到指定演员");
        return null;
    }
}

[Serializable]
public struct Lines {
    public string ActorName;
    public string Sentence;
    public List<string> Chioces;

    public Lines(string actorName,string sentence) {
        ActorName = actorName;
        Sentence = sentence;
        Chioces = new();
    }

    public Lines(string actorName,string sentence,List<string> chioces) {
        ActorName = actorName;
        Sentence = sentence;
        Chioces = chioces;
    }
}
