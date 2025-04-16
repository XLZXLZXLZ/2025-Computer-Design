using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class EntryManager : MonoBehaviour
{
    public void ChooseType(int playerType)
    {
        PlayerPrefs.SetInt("PlayerType", playerType);
        Cover.Instance.ChangeScene("MainMenu");
    }
}
