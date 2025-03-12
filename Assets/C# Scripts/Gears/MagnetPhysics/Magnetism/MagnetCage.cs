using DG.Tweening;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MagnetCage : MonoBehaviour
{
    public bool success { get; private set; }

    [SerializeField] Transform lid;

    private void Awake()
    {
        success = false;
    }

    // Start is called before the first frame update

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (!success)
        {
            if(collision.TryGetComponent<PickableMagnet>(out var pm))
                Collected(pm);
        }
    }

    private void Collected(PickableMagnet pm)
    {
        if (pm)
        {
            pm.ShutDownMagnet();
            success = true;
            AudioManager.Instance.PlaySe("Caged");
        }

        lid.DORotate(Vector3.forward * -180, 0.5f).SetEase(Ease.OutQuad);

        var s = transform.localScale.x;

        DOTween.Sequence()
            .Append(transform.DOScale(s * 1.2f , 0.2f))
            .Append(transform.DOScale(s, 0.2f));
        
    }
}
