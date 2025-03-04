using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class LevelManager : Singleton<LevelManager>
{
    public void FinishLevel()
    {
        Cover.Instance.ChangeScene("ChooseLevelMenu");
    }

    public void RestartLevel()
    {
        Cover.Instance.ChangeScene(SceneManager.GetActiveScene().name);
    }
}
