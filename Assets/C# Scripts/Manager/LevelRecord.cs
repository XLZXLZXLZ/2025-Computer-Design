using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

[Serializable]
public class LevelData{
    public string LevelName;
    public int StarCount;
    public bool Accessable;

    public LevelData(string _levelname,int _starcount,bool _accessable){ 
        LevelName = _levelname;
        StarCount = _starcount;
        Accessable = _accessable;
    }
}

[CreateAssetMenu(fileName = "LevelRecord", menuName = "ScriptableObject/LevelRecord", order = 0)]
public class LevelRecord : ScriptableObject
{
    public List<LevelData> LevelDatas = new List<LevelData>();

    public void ResetData() {
        Debug.Log($"LevelData:{name}Reseted");
        LevelDatas = new();
        int sceneCount = SceneManager.sceneCountInBuildSettings;
        for(int i=0;i < sceneCount;i++){
            string scenePath = SceneUtility.GetScenePathByBuildIndex(i);
            string sceneName = System.IO.Path.GetFileNameWithoutExtension(scenePath);
            if (sceneName.StartsWith("Level") && !sceneName.Contains("old")) {
                if (this.name == "LevelRecord"){
                    if (sceneName.Equals("Level1-1"))
                        LevelDatas.Add(new LevelData(sceneName, 0, true));
                    else
                        LevelDatas.Add(new LevelData(sceneName, 0, false));
                } else
                    LevelDatas.Add(new LevelData(sceneName, 3, true));
            }
        }
    }
}
