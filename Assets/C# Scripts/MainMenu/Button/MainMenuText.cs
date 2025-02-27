using DG.Tweening;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class MainMenuText : MonoBehaviour
{
    [SerializeField] private float fadeDuration = 0.2f;

    private string currentText;
    private Text text;
    private bool animating = false;
    private bool ShouldChangeText => currentText != text.text && !animating;

    private void Awake()
    {
        text = GetComponent<Text>();
    }

    private void Update()
    {
        if(ShouldChangeText)
        {
            ChangeText();
        }
    }

    private void ChangeText()
    {
        animating = true;
        DOTween.Sequence()
            .Append(text.DOFade(0, fadeDuration))
            .AppendCallback(() => text.text = currentText)
            .Append(text.DOFade(1, fadeDuration))
            .AppendCallback(() => animating = false);
    }

    public void SwitchText(string newText)
    {
        currentText = newText;
    }
}
