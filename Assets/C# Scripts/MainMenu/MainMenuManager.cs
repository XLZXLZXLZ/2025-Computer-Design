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

    public MainMenuText Title => title;
    public MainMenuText Description => description;

    private int currentChooseChapter = -1;
    private int currentChooseLevel = -1;


/*    protected override void Awake()
    {
        base.Awake();
        //AudioManager.Instance.PlayBgm();
    }*/



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
