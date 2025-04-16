using DG.Tweening;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class StartMenu : MonoBehaviour
{
    [SerializeField]
    private float startDelay = 1f;

    public void Start()
    {
        for(int i = 0; i < transform.childCount; i++)
        {
            var t = transform.GetChild(i).GetComponent<RectTransform>();
            DOTween.Sequence()
                .AppendInterval(startDelay + i * 0.4f)
                .Append(t.DOScale(1f,1f).SetEase(Ease.OutBack));
            t.localScale = Vector3.zero;
        }
    }

    public void StartGame()
    {
        Cover.Instance.ChangeScene("ChooseLevelMenu");
    }

    public void ExitGame()
    {
        Application.Quit();
    }
}
