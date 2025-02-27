using DG.Tweening;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

public class ChooseLevel : MonoBehaviour, IPointerEnterHandler, IPointerExitHandler
{
    [SerializeField] private string title;
    [SerializeField][Multiline(5)] private string description;
    [SerializeField] private float fadeDuration = 0.3f;

    public void OnPointerEnter(PointerEventData eventData)
    {
        MainMenuManager.Instance.Title.SwitchText(title);
        MainMenuManager.Instance.Description.SwitchText(description);
    }

    public void OnPointerExit(PointerEventData eventData)
    {
        MainMenuManager.Instance.Title.SwitchText("");
        MainMenuManager.Instance.Description.SwitchText("");
    }
}
