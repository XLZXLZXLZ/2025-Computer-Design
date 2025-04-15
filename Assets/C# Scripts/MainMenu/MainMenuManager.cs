using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

// 主菜单管理类，继承自单例模式基类，用于管理主菜单界面逻辑
public class MainMenuManager : Singleton<MainMenuManager>
{
    [SerializeField]
    private MainMenuText title;          // 菜单标题文本组件
    [SerializeField]
    private MainMenuText description;    // 菜单描述文本组件
    [SerializeField]
    private ChapterPanel ChapterPanel;   // 章节选择面板组件

    // 属性访问器
    public MainMenuText Title => title;
    public MainMenuText Description => description;

    // 当前选择的章节和关卡索引（-1表示未选择状态）
    private int currentChooseChapter = -1;
    private int currentChooseLevel = -1;

    private void Start()
    {
        // 初始化方法保留位（可添加启动逻辑）
    }

    /// <summary>
    /// 初始化关卡数据并更新UI状态
    /// 1. 默认禁用所有章节按钮
    /// 2. 根据关卡解锁状态更新对应按钮的交互状态和颜色
    /// </summary>
    public void InitialLevelData()
    {
        // 初始化前3个章节按钮为禁用状态（灰色）
        for (int i = 0; i < 3; i++)
        {
            var chapterBtn = ChapterPanel.transform.GetChild(i);
            chapterBtn.GetChild(0).GetComponent<Image>().color = new Color(1, 1, 1, 0.3f); // 章节按钮(小恐龙)半透明
            chapterBtn.GetComponent<Button>().interactable = false; // 禁用交互

            var perLevel = transform.GetChild(2 + i);//2为层级中偏移
            //初始化每个小关的选择按钮为不可见
            for(int j=0;j< 4; j++){ 
                perLevel.GetChild(j).gameObject.SetActive(false);
            }
        }

        // 遍历所有关卡数据配置
        foreach (var data in LevelChooseManager.Instance.LevelRecords.LevelDatas)
        {
            // 解析关卡名称格式（示例："Level1-3" -> 章节1 关卡3）
            string[] chapter_levelInfo = data.LevelName.Replace("Level", "").Split("-");
            int chapter = int.Parse(chapter_levelInfo[0]);
            int level = int.Parse(chapter_levelInfo[1]);

            if (data.Accessable) // 如果关卡已解锁
            {

                // 激活对应章节按钮
                var chapterBtn = ChapterPanel.transform.GetChild(chapter - 1);
                chapterBtn.GetChild(0).GetComponent<Image>().color = Color.white;    // 白色
                chapterBtn.GetComponent<Button>().interactable = true;  // 启用交互

                // 激活对应关卡按钮（+1偏移量可能因UI层级结构）
                Transform levelBtn = transform.GetChild(chapter + 1).GetChild(level - 1);
                levelBtn.GetComponent<Image>().color = Color.white;
                levelBtn.GetComponent<Button>().interactable = true;
            }
            else // 关卡未解锁
            {
                // 禁用对应关卡按钮
                Transform levelBtn = transform.GetChild(chapter + 1).GetChild(level - 1);
                levelBtn.GetChild(1).GetComponent<Image>().color = new Color(1, 1, 1, 0.3f);
                levelBtn.GetComponent<Button>().interactable = false;
            }
        }
    }

    /// <summary>
    /// 当按下小恐龙按钮时，之战时当前章节的关卡选择按钮，否则会遮挡导致UI显示不正确
    /// </summary>
    public void ShowLevelBtn(int chapter){
        // 关闭其他章节的关卡选择按钮
        Transform perLevel;
        for (int i=1;i <= 3;i++){
            if (i == chapter) continue;
            perLevel = transform.GetChild(1 + i);//1为层级中偏移
            for (int j = 0; j < 4; j++) {
                perLevel.GetChild(j).gameObject.SetActive(false);
            }
        }
        
        // 激活当前章节的关卡选择按钮
        perLevel = transform.GetChild(1 + chapter);//1为层级中偏移
        for (int j = 0; j < 4; j++) {
            perLevel.GetChild(j).gameObject.SetActive(true);
        }
    }

    /// <summary>
    /// 记录当前选择的关卡索引
    /// </summary>
    public void ChooseLevel(int level)
    {
        currentChooseLevel = level;
    }

    /// <summary>
    /// 记录当前选择的章节索引
    /// </summary>
    public void ChooseChapter(int chapter)
    {
        currentChooseChapter = chapter;
    }

    /// <summary>
    /// 启动游戏场景加载
    /// 根据选择的章节关卡生成场景名称（示例："Level2-1"）
    /// </summary>
    public void StartGame()
    {
        string sceneName = $"Level{currentChooseChapter}-{currentChooseLevel}";
        Cover.Instance.ChangeScene(sceneName); // 通过场景过渡管理器加载场景
    }
}
