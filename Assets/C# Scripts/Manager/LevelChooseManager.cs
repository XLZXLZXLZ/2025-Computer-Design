using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class LevelChooseManager : Singleton<LevelChooseManager>
{
    protected override bool IsDonDestroyOnLoad => true;

    [SerializeField] public LevelRecord LevelRecords;
    [SerializeField] string DataPath = "LevelRecord/LevelRecord";

    float CheatTimer = 0f;
    float ResetTimer = 0f;

    protected override void Awake() {
        base.Awake();
        LevelRecords = Resources.Load<LevelRecord>(DataPath);
        if(PlayerPrefs.GetInt("PlayerType",0) == 2)
        {
            LevelRecords = Resources.Load<LevelRecord>("LevelRecord/Cheat");
        }
        MainMenuManager.Instance.InitialLevelData();
    }

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        if (Input.GetKey(KeyCode.R))
        {
            ResetTimer += Time.deltaTime;
            if (ResetTimer > 3)
            {
                LevelRecords.ResetData();
                ResetTimer = 0;
            }
        }
        else { ResetTimer = 0; }
    }

    private void FixedUpdate() {
        if (Input.GetKey(KeyCode.R)) {
            ResetTimer += Time.fixedDeltaTime;
            if (ResetTimer > 3) {
                LevelRecords.ResetData();
                ResetTimer = 0;
            }
        } else { ResetTimer = 0; }

        if (Input.GetKey(KeyCode.CapsLock)) {
            CheatTimer += Time.fixedDeltaTime;
            if (CheatTimer > 3) {
                LevelRecords = Resources.Load<LevelRecord>("LevelRecord/Cheat");
                CheatTimer = 0;
            }
        } else { CheatTimer = 0; }
    }

    public void OnCheckPointPassed(ILevelSettlement SL){
        int starCount = SL.Score;
        var curLevel = SceneManager.GetActiveScene();
        var LevelRecord = LevelRecords.LevelDatas.Find((x) => x.LevelName.Equals(curLevel.name));
        LevelRecord.StarCount = starCount;
        var sceneCount = LevelRecords.LevelDatas.IndexOf(LevelRecord);
        if(sceneCount < LevelRecords.LevelDatas.Count-1){ 
            LevelRecords.LevelDatas[sceneCount+1].Accessable = true;
        }
    }

    public int GetStarCount(int chapter,int level){ 
        string info = "Level" + chapter.ToString() + '-' + level.ToString();
        var record = LevelRecords.LevelDatas.Find((x) => x.LevelName.Equals(info));
        return record.StarCount;
    }
}
