using DG.Tweening;
using UnityEngine;

public class Elevator : Gear
{
    [SerializeField] private Vector3 relativePos;
    [SerializeField] private Vector3 targetAngle;
    [SerializeField] private float moveTime = 1f;
    [SerializeField] private bool usePhysics = true;

    private Rigidbody2D rb;
    private Vector3 originPos;
    private Vector3 originAngle;

    protected override void Awake()
    {
        base.Awake();
        rb = GetComponent<Rigidbody2D>();

        originPos = transform.position;
        originAngle = transform.eulerAngles;
    }

    protected override void SwitchOn()
    {
        base.SwitchOn();
        MoveToTarget(originPos + relativePos, targetAngle);
        AudioManager.Instance.PlaySe("ElevatorMove");
    }

    protected override void SwitchOff()
    {
        base.SwitchOff();
        MoveToTarget(originPos, originAngle);
        AudioManager.Instance.PlaySe("ElevatorMove");
    }

    private void MoveToTarget(Vector3 targetPos, Vector3 targetEuler)
    {
        if (usePhysics)
        {
            // 物理模式移动
            DOTween.To(() => rb.position, (Vector2 x) => rb.MovePosition(x), targetPos, moveTime)
                .SetEase(Ease.InQuad);

            transform.DORotate(targetEuler, moveTime);
        }
    }

    protected override void OnDrawGizmos()
    {
        base.OnDrawGizmos();
        Gizmos.color = Color.yellow;
        Gizmos.DrawLine(transform.position, transform.position + relativePos);
        Gizmos.DrawWireSphere(transform.position + relativePos, 0.5f);
    }
}