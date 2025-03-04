using DG.Tweening;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using Unity.VisualScripting;
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
    [SerializeField] List<GameObject> promptObject = new();//给玩家的提示物体
    //配置提示物体时用到的计数器

    [Tooltip("打字机效果时间间隔")]
    [SerializeField] float typingSpeed;//打字机效果时间间隔
    [SerializeField] KeyCode nextLineKey = KeyCode.K;
    [SerializeField] KeyCode skipCurrentLineKey = KeyCode.L;

    [SerializeField] float fadeInLength = 100f; //做移入动画时的移动距离
    [SerializeField] GameObject panel;

    private Vector3 startPos;

    private bool Interact => Input.GetKeyDown(nextLineKey) || Input.GetMouseButtonDown(0);

    Coroutine TypingWords;
    bool waitForButtonClick = false;

    private void OnEnable()
    {
        LoadDialogueConfig();
        currentLineIndex = 0;
    }

    // Start is called before the first frame update
    void Start()
    {
        startPos = panel.transform.position;
        panel.transform.position = startPos + Vector3.down * fadeInLength;
    }

    // Update is called once per frame
    void Update()
    {
        if (Interact && TypingWords == null && !waitForButtonClick)
        {
            PushDialogue();
        }
        else if (Interact && TypingWords != null) {
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

        nextLine.StartPrompt();
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

        for (int i=0;i < words.Count() + 1; i++) { 
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
        int promptObjectIndex = 0;

        string[] blocks = DialogueConfig.Dialogue.text.Split('~', System.StringSplitOptions.None);
        foreach (var block in blocks) {
            string lines = block;
            string[] linesArray = lines.Split(new[] { '\n', '\r' }, System.StringSplitOptions.RemoveEmptyEntries);
            for (int i=0;i < linesArray.Count() ; i++) { 
                string line = linesArray[i];

                if (line.StartsWith("@")) { 
                    //读取到演员信息
                    string ActorName = line.Substring(1);
                    if (DialogueConfig.Actors.IfActorExistInConfig(ActorName))
                    {
                        i++;
                        line = linesArray[i];
                        var newLine = new Lines(ActorName, line,null);
                        if (i < linesArray.Count() - 1 && linesArray[i + 1].StartsWith("#"))
                        {
                            i++;
                            for (; i < linesArray.Count() && linesArray[i].StartsWith("#"); i++)
                            {
                                newLine.Chioces.Add(linesArray[i].Substring(1));
                            }
                        }
                        else {
                            //i++;
                        } 

                        if (i <= linesArray.Count() - 1 && linesArray[i].StartsWith("*")) {
                            line = linesArray[i].Substring(1);
                            SingleDialoguePromptType promptType = SingleDialoguePromptType.Point;
                            try {
                                Enum.TryParse(line, out promptType);
                            } catch (Exception e) {
                                Debug.LogError("无法从文本转换到枚举,检查拼写");
                                Debug.LogException(e);
                            }

                            if (promptObjectIndex >= promptObject.Count) {
                                Debug.LogError("错误的玩家提示相关物体配置");
                                promptObjectIndex = 0;
                            }
                            Debug.Log("注册提示事件");
                            newLine = new Lines(newLine, () => SingleDialoguePrompt.Interact(this, promptObject[promptObjectIndex++], promptType));
                        }

                        Dialogue.Add(newLine);
                    } else {
                        Debug.LogError("读取到不存在的演员信息");
                        EndDialogue();  
                    }
                }
            }
        }

        //string[] lines = DialogueConfig.Dialogue.text.Split(new[] { '\n', '\r' }, System.StringSplitOptions.RemoveEmptyEntries);
        //for (int i=0;i < lines.Count(); i++)
        //{
        //    string line = lines[i];
        //    if (line.StartsWith("@")) { 
        //        string ActorName = line.Substring(1);
        //        if (DialogueConfig.Actors.IfActorExistInConfig(ActorName))
        //        {
        //            i++;
        //            line = lines[i];
        //            var newLine = new Lines(ActorName, line,null);
        //            Dialogue.Add(newLine);
        //            if (i < lines.Count() - 1 && lines[i + 1].StartsWith("#")) {
        //                i++;
        //                for (;i < lines.Count() && lines[i].StartsWith("#"); i++)
        //                {
        //                    Debug.Log("检测到按钮");
        //                    newLine.Chioces.Add(lines[i].Substring(1));
        //                }
        //            }
        //        } else {
        //            Debug.LogError("读取到不存在的演员信息");
        //            EndDialogue();
        //        }
        //    }
        //}
    }

    void EndDialogue() {
        //结束对话
        Dialogue.Clear();

        //关闭对话框

        panel.transform.DOMove(startPos + Vector3.down * fadeInLength, 0.5f)
            .SetEase(Ease.InBack)
            .OnComplete(() => gameObject.SetActive(false));
    }

    void StartDialogue()
    {
        panel.transform.DOMove(startPos, 0.5f)
            .SetEase(Ease.OutBack)
            .OnComplete(() => ShowNextLine(Dialogue[currentLineIndex]));
    }

    //外部激活API
    public void Activate()
    {
        StartDialogue();
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
    public event Action Prompt;

    public void StartPrompt() {
        Prompt?.Invoke();
    } 

    public Lines(string actorName,string sentence,Action action) {
        ActorName = actorName;
        Sentence = sentence;
        Chioces = new();
        Prompt = action;
    }

    public Lines(string actorName,string sentence,List<string> chioces,Action action) {
        ActorName = actorName;
        Sentence = sentence;
        Chioces = chioces;
        Prompt = action;
    }

    public Lines(Lines l,Action action) { 
        ActorName = l.ActorName;
        Sentence = l.Sentence;
        Chioces = l.Chioces;
        Prompt = action;
    }
}
