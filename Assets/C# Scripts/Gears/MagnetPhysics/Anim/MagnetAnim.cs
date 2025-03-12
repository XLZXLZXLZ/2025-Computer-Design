using DG.Tweening;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MagnetAnim : MonoBehaviour
{
    MouseFollowMagnet parent;
    private Color startColor;

    private void Awake()
    {
        parent = GetComponentInParent<MouseFollowMagnet>();
        parent.changeApplyState += OnApplyStateChange;
        startColor = GetComponent<ParticleSystem>().main.startColor.color;
    }
    private void Update()
    {
        var ps = GetComponent<ParticleSystem>().main;
        (float r, float g, float b) = (startColor.r, startColor.g, startColor.b);

        ps.startColor = new Color(0, 0, 0, parent.IsApplying ? 0.3f : 0f);
    }
    private void OnApplyStateChange(bool applyState)
    {
        transform.DOScale(applyState ? 1f : 0f, 0.3f).SetEase(Ease.OutQuad);
    }
}
