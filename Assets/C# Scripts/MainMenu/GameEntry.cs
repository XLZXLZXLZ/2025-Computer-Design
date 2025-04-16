using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class GameEntry : MonoBehaviour
{
    private void Awake()
    {
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
