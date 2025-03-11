using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.SceneManagement;

public class LevelManager : Singleton<LevelManager>
{
    public void FinishLevel()
    {
        AudioManager.Instance.PlaySe("FinishLevel");

        string currentScene = SceneManager.GetActiveScene().name;
        string[] sceneParts = currentScene.Split('-');

        // 解析关卡数字
        if (sceneParts.Length >= 2 && int.TryParse(sceneParts.Last(), out int currentLevel))
        {
            // 构建下一关名称
            string baseName = string.Join("-", sceneParts.Take(sceneParts.Length - 1));
            string nextScene = $"{baseName}-{currentLevel + 1}";

            // 检查场景是否存在
            if (SceneExists(nextScene))
            {
                Cover.Instance.ChangeScene(nextScene);
                return;
            }
        }

        // 没有下一关时返回选关界面
        Cover.Instance.ChangeScene("ChooseLevelMenu");
    }

    public void RestartLevel()
    {
        Cover.Instance.ChangeScene(SceneManager.GetActiveScene().name);
    }
    // 检查场景是否存在于Build Settings
    private bool SceneExists(string sceneName)
    {
        for (int i = 0; i < SceneManager.sceneCountInBuildSettings; i++)
        {
            var scenePath = SceneUtility.GetScenePathByBuildIndex(i);
            var lastSlash = scenePath.LastIndexOf("/");
            var sceneNameInBuild = scenePath.Substring(lastSlash + 1, scenePath.LastIndexOf(".unity") - lastSlash - 1);

            if (sceneNameInBuild == sceneName)
                return true;
        }
        return false;
    }


}
