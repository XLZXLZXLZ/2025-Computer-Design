using DG.Tweening;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Elevator : Gear
{
    [SerializeField]
    private Vector3 targetPos;

    [SerializeField]
    private float moveTime = 1f;

    private Vector3 origin;

    protected override void Awake()
    {
        base.Awake();

        origin = transform.position;
        targetPos += transform.position;
    }

    protected override void SwitchOn()
    {
        base.SwitchOn();

        transform.DOMove(targetPos, moveTime).SetEase(Ease.InQuad);
    }

    protected override void SwitchOff()
    {
        base.SwitchOff();

        transform.DOMove(origin, moveTime).SetEase(Ease.InQuad);
    }

    protected override void OnDrawGizmos()
    {
        base.OnDrawGizmos();

        Gizmos.color = Color.yellow;

        Gizmos.DrawLine(transform.position + targetPos, transform.position);
        Gizmos.DrawWireSphere(transform.position + targetPos, 0.5f);
    }
}
