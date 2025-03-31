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

    private void Awake() {
        base.Awake();
        LevelRecords = Resources.Load<LevelRecord>(DataPath);
        MainMenuManager.Instance.InitialLevelData();
    }

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    private void FixedUpdate() {
        if (Input.GetKey(KeyCode.R)) {
            ResetTimer += Time.fixedDeltaTime;
            if (ResetTimer > 3) {
                LevelRecords.Reset();
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
}
