using DG.Tweening;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ChapterPanel : MonoBehaviour
{
    [SerializeField]
    private LevelPanel[] levelPanels;

    private bool animating = false;

    private void Awake()
    {
        foreach (LevelPanel levelPanel in levelPanels)
        {
            levelPanel.Init(this);
        }
    }

    public void ChooseChapter(int choice)
    {
        if(animating)
            return;
        StartCoroutine(ChooseChapterEnumrator(choice));
        AudioManager.Instance.PlaySe("ChooseLevel");
    }

    private IEnumerator ChooseChapterEnumrator(int choice)
    {
        animating = true;
        MainMenuManager.Instance.ChooseChapter(choice);
        Hide();
        yield return new WaitForSeconds(1f);
        levelPanels[choice - 1].Show();
        animating = false;
    }

    public void Show()
    {
        for (int i = 0; i < transform.childCount; i++)
        {
            DOTween.Sequence()
                 .AppendInterval(i * 0.2f)
                 .Append(transform.GetChild(i).DOScale(1f, 0.6f).SetEase(Ease.OutBack));
        }
    }

    public void Hide()
    {
        for (int i = 0; i < transform.childCount; i++)
        {
            DOTween.Sequence()
                 .AppendInterval(i * 0.2f)
                 .Append(transform.GetChild(i).DOScale(0f, 0.6f).SetEase(Ease.InBack));
        }
    }


}
