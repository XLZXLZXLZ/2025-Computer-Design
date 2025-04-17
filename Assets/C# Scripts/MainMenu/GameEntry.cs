using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class GameEntry : MonoBehaviour
{
    private void Awake()
    {
        if(Application.platform == RuntimePlatform.WebGLPlayer)
        {
            Cover.Instance.ChangeScene("MainMenu");
            PlayerPrefs.SetInt("PlayerType", 2);
            return;
        }

        if (PlayerPrefs.GetInt("PlayerType", 0) != 0)
        {
            Cover.Instance.ChangeScene("MainMenu");
        }
        else
        {
            Cover.Instance.ChangeScene("ChoosePlayerStyle");
        }
    }
}
