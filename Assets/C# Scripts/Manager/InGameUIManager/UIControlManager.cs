using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class UIControlManager : MonoBehaviour
{
    public void Pause()
    {

    }

    public void Restart()
    {
        Cover.Instance.ChangeScene(SceneManager.GetActiveScene().name);
        AudioManager.Instance.PlaySe("ChooseLevel");
    }

    public void Exit()
    {
        Cover.Instance.ChangeScene("ChooseLevelMenu");
        AudioManager.Instance.PlaySe("ChooseLevel");
    }
}
