using DG.Tweening;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class StartMenu : MonoBehaviour
{
    [SerializeField]
    private float startDelay = 1f;

    [SerializeField]
    private Text playerTypeText;

    public void Start()
    {
        for(int i = 0; i < transform.childCount; i++)
        {
            var t = transform.GetChild(i).GetComponent<RectTransform>();
            DOTween.Sequence()
                .AppendInterval(startDelay + i * 0.4f)
                .Append(t.DOScale(transform.localScale,1f).SetEase(Ease.OutBack));
            t.localScale = Vector3.zero;
        }

        playerTypeText.text = PlayerPrefs.GetInt("PlayerType", 0) == 2 ? "评委模式" : "标准模式";
    }

    public void StartGame()
    {
        Cover.Instance.ChangeScene("ChooseLevelMenu");
    }

    public void ExitGame()
    {
        Application.Quit();
    }

    public void ChangePlayerType()
    {
        if (Application.platform == RuntimePlatform.WebGLPlayer)
        {
            return;
        }

            var t = PlayerPrefs.GetInt("PlayerType", 0);
        t = t % 2 + 1;
        PlayerPrefs.SetInt("PlayerType", t);
        playerTypeText.text = t == 1 ? "标准模式" : "评委模式";
    }
}
