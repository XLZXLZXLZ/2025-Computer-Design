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

    [Tooltip("打字机效果时间间隔")]
    [SerializeField] float typingSpeed;//打字机效果时间间隔
    [SerializeField] KeyCode nextLineKey = KeyCode.K;
    [SerializeField] KeyCode skipCurrentLineKey = KeyCode.L;

    Coroutine TypingWords;

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
        if (Input.GetKeyDown(nextLineKey) && TypingWords == null) {
            if (currentLineIndex == Dialogue.Count - 1) {
                EndDialogue();
                return;
            }
            ShowNextLine(Dialogue[++currentLineIndex]);
        } else if (Input.GetKeyDown(skipCurrentLineKey) && TypingWords != null) {
            EndType();
        }
    }

    /// <summary>
    /// 显示下一行对话
    /// </summary>
    /// <param name="nextLine">下一行对话的数据</param>
    void ShowNextLine(Lines nextLine) {
        ActorIcon.sprite = DialogueConfig.Actors.GetIconWithName(nextLine.ActorName);
        ActorName.text = nextLine.ActorName;
        TypingWords = StartCoroutine(TypeWords(nextLine.Sentence));
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
                    Dialogue.Add(new Lines { ActorName = ActorName, Sentence = line });
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
}
