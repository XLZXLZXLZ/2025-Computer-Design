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

    [Tooltip("���ֻ�Ч��ʱ����")]
    [SerializeField] float typingSpeed;//���ֻ�Ч��ʱ����
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
    /// ��ʾ��һ�жԻ�
    /// </summary>
    /// <param name="nextLine">��һ�жԻ�������</param>
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
                    Debug.LogError("��ȡ�������ڵ���Ա��Ϣ");
                    EndDialogue();
                }
            }
        }
    }

    void EndDialogue() {
        //�����Ի�
        Dialogue.Clear();

        //�رնԻ���

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
        Debug.LogError("δ���б����ҵ�ָ����Ա");
        return null;
    }
}

[Serializable]
public struct Lines {
    public string ActorName;
    public string Sentence;
}
