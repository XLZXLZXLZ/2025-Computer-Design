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
    [SerializeField] List<GameObject> promptObject = new();//����ҵ���ʾ����
    //������ʾ����ʱ�õ��ļ�����

    [Tooltip("���ֻ�Ч��ʱ����")]
    [SerializeField] float typingSpeed;//���ֻ�Ч��ʱ����
    [SerializeField] KeyCode nextLineKey = KeyCode.K;
    [SerializeField] KeyCode skipCurrentLineKey = KeyCode.L;

    [SerializeField] float fadeInLength = 100f; //�����붯��ʱ���ƶ�����
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
    /// ��ʾ��һ�жԻ�
    /// </summary>
    /// <param name="nextLine">��һ�жԻ�������</param>
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
                    //��ȡ����Ա��Ϣ
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
                                Debug.LogError("�޷����ı�ת����ö��,���ƴд");
                                Debug.LogException(e);
                            }

                            if (promptObjectIndex >= promptObject.Count) {
                                Debug.LogError("����������ʾ�����������");
                                promptObjectIndex = 0;
                            }
                            Debug.Log("ע����ʾ�¼�");
                            newLine = new Lines(newLine, () => SingleDialoguePrompt.Interact(this, promptObject[promptObjectIndex++], promptType));
                        }

                        Dialogue.Add(newLine);
                    } else {
                        Debug.LogError("��ȡ�������ڵ���Ա��Ϣ");
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
        //                    Debug.Log("��⵽��ť");
        //                    newLine.Chioces.Add(lines[i].Substring(1));
        //                }
        //            }
        //        } else {
        //            Debug.LogError("��ȡ�������ڵ���Ա��Ϣ");
        //            EndDialogue();
        //        }
        //    }
        //}
    }

    void EndDialogue() {
        //�����Ի�
        Dialogue.Clear();

        //�رնԻ���

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

    //�ⲿ����API
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
        Debug.LogError("δ���б����ҵ�ָ����Ա");
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
