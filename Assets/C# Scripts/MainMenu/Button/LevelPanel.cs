using DG.Tweening;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LevelPanel : MonoBehaviour
{
    private ChapterPanel chapterPanel;
    private bool animating = false;

    public void Init(ChapterPanel chapterPanel)
    {
        this.chapterPanel = chapterPanel;
        for (int i = 0; i < transform.childCount; i++)
        {
             transform.GetChild(i).localScale = Vector3.zero;
        }
    }

    public void Back()
    {
        if(animating)
            return;
        StartCoroutine(BackEnumrator());
        AudioManager.Instance.PlaySe("ChooseLevel");
    }

    public void ChooseLevel(int choice)
    {
        if (animating)
            return;
        StartCoroutine(ChooseLevelEnumrator(choice));
        AudioManager.Instance.PlaySe("ChooseLevel");
    }

    private IEnumerator BackEnumrator()
    {
        animating = true;
        Hide();
        yield return new WaitForSeconds(1.2f);
        chapterPanel.Show();
        animating = false;
    }

    private IEnumerator ChooseLevelEnumrator(int choice)
    {
        animating = true;
        MainMenuManager.Instance.ChooseLevel(choice);
        Hide();
        yield return new WaitForSeconds(2f);
        MainMenuManager.Instance.StartGame();
        animating = false;
    }

    public void Show()
    {
        for (int i = 0; i < transform.childCount; i++)
        {
            DOTween.Sequence()
                 .AppendInterval(i * 0.15f)
                 .Append(transform.GetChild(i).DOScale(1f, 0.4f).SetEase(Ease.OutBack));
        }
    }

    public void Hide()
    {
        for (int i = 0; i < transform.childCount; i++)
        {
            DOTween.Sequence()
                 .AppendInterval(i * 0.15f)
                 .Append(transform.GetChild(i).DOScale(0f, 0.4f).SetEase(Ease.InBack));
        }
    }
}
