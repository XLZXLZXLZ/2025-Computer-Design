using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class MainMenuManager : Singleton<MainMenuManager>
{
    [SerializeField]
    private MainMenuText title;
    [SerializeField]
    private MainMenuText description;
    [SerializeField]
    private ChapterPanel ChapterPanel;

    public MainMenuText Title => title;
    public MainMenuText Description => description;

    private int currentChooseChapter = -1;
    private int currentChooseLevel = -1;


    /*    protected override void Awake()
        {
            base.Awake();
            //AudioManager.Instance.PlayBgm();
        }*/


    private void Start() {
        
    }

    //加载关卡数据，关闭一些按钮
    public void InitialLevelData(){
        int chapter;
        int level;
        string temp;
        string[] chapter_levelInfo;
        for(int i=0;i < 3;i++){ 
            var c = ChapterPanel.transform.GetChild(i);
            c.GetComponent<Image>().color = new Vector4(0.5f,0.5f,0.5f,1);
            c.GetComponent<Button>().interactable = false;
        }

        foreach(var data in LevelChooseManager.Instance.LevelRecords.LevelDatas){
            temp = new(data.LevelName);
            chapter_levelInfo = temp.Replace("Level", "").Split("-");
            chapter = int.Parse(chapter_levelInfo[0]);
            level = int.Parse(chapter_levelInfo[1]);
            if(data.Accessable){ 
                ChapterPanel.transform.GetChild(chapter-1).GetComponent<Image>().color = new Vector4(1,1,1,1);
                ChapterPanel.transform.GetChild(chapter-1).GetComponent<Button>().interactable = true;

                transform.GetChild(chapter+1).GetChild(level-1).GetComponent<Image>().color = new Vector4(1,1,1,1);
                transform.GetChild(chapter+1).GetChild(level-1).GetComponent<Button>().interactable = true;
            } else {
                transform.GetChild(chapter + 1).GetChild(level - 1).GetComponent<Image>().color = new Vector4(0.5f, 0.5f, 0.5f, 1);
                transform.GetChild(chapter + 1).GetChild(level - 1).GetComponent<Button>().interactable = false;
            }
        }
    }

    public void ChooseLevel(int level)
    {
        currentChooseLevel = level;
    }

    public void ChooseChapter(int chapter)
    {
        currentChooseChapter = chapter;
    }

    public void StartGame()
    {
        var sceneName = "Level" + currentChooseChapter.ToString() + "-" + currentChooseLevel.ToString();
        Cover.Instance.ChangeScene(sceneName);
    }


}
