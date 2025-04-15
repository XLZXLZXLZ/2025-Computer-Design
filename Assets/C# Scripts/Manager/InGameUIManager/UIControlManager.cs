using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class UIControlManager : MonoBehaviour
{
    public bool Puasing = false;

    public void Pause()
    {
        FindAnyObjectByType<TipPanel>().ShowTip();
        Puasing = true;
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
