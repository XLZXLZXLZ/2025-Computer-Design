using DG.Tweening;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

public class ChooseLevel : MonoBehaviour, IPointerEnterHandler, IPointerExitHandler
{
    [SerializeField] private string chapter_levelInfo;
    [SerializeField] private string title;
    [SerializeField][Multiline(5)] private string description;
    [SerializeField] private float fadeDuration = 0.3f;
    Animator animator;

    void Start(){
        var info = chapter_levelInfo.Split("-");
        int chapter = int.Parse(info[0]);
        int level = int.Parse(info[1]);
        int starCount = LevelChooseManager.Instance.GetStarCount(chapter,level);
        for(int i=starCount;i < 3;i++){
            transform.Find("StarHolder").GetChild(i).gameObject.SetActive(false);
        }
        animator = GetComponent<Animator>();
    }

    public void OnPointerEnter(PointerEventData eventData)
    {
        MainMenuManager.Instance.Title.SwitchText(title);
        MainMenuManager.Instance.Description.SwitchText(description);
        ShowStar();
    }

    public void OnPointerExit(PointerEventData eventData)
    {
        MainMenuManager.Instance.Title.SwitchText("");
        MainMenuManager.Instance.Description.SwitchText("");
        HideStar();
    }

    void ShowStar(){
        animator.Play("ShowStar");
    }

    void HideStar(){
        animator.Play("HideStar");
    }

    public void SetUnlockState(bool isUnlocked)
    {

    }
}
