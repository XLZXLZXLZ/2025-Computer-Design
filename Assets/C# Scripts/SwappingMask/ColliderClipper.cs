using DG.Tweening;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ColliderClipper : Singleton<ColliderClipper>
{
    [SerializeField]
    private float windowSize = 4;

    private void Start()
    {
        transform.localScale = Vector3.zero;
    }

    /*
    private void ResetPosition(Vector3 pos)
    {
        if(isResetting) 
            return;

        isResetting = true;
        DOTween.Sequence()
            .Append(transform.DOScale(0, 0.2f))
            .AppendInterval(0.5f)
            .AppendCallback(() => transform.position = pos)
            .Append(transform.DOScale(windowSize, 0.25f)).SetEase(Ease.OutQuad)
            .Join(transform.DOBlendableRotateBy(Vector3.forward * 90, 0.25f)).SetEase(Ease.OutQuad)
            .AppendCallback(() => isResetting = false);
    }
    */

    public Tween CloseWindow()
    {
         return transform.DOScale(0, 0.2f);
    }

    public Tween SetWindow(Vector3 pos)
    {
        return DOTween.Sequence()
           .AppendCallback(() => transform.position = pos)
           .Append(transform.DOScale(windowSize, 0.25f)).SetEase(Ease.OutQuad)
           .Join(transform.DOBlendableRotateBy(Vector3.forward * 90, 0.25f)).SetEase(Ease.OutQuad);
    }

    public Vector2[][] GetWindow()
    {
        var window = new Vector2[1][];
        window[0] = new Vector2[4];
        window[0][0] = (Vector2)transform.position + Vector2.up * transform.localScale * 0.5f + Vector2.right * transform.localScale * 0.5f;
        window[0][1] = (Vector2)transform.position - Vector2.up * transform.localScale * 0.5f + Vector2.right * transform.localScale * 0.5f;
        window[0][2] = (Vector2)transform.position - Vector2.up * transform.localScale * 0.5f - Vector2.right * transform.localScale * 0.5f;
        window[0][3] = (Vector2)transform.position + Vector2.up * transform.localScale * 0.5f - Vector2.right * transform.localScale * 0.5f;
        return window;
    }

    private void Update()
    {
        
    }
}
