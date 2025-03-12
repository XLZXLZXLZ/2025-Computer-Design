using System;
using UnityEngine;

public class MouseFollowMagnet : MonoBehaviour
{
    [Header("Magnet Settings")]
    public float radius = 5f;
    public float minForce = 1f;
    public float maxForce = 100f;
    public float forceFactor = 100f;
    public LayerMask anchorLayer;
    public Color gizmoColor = Color.green;

    [SerializeField] float dampingFactor = 5f;    // 阻尼系数
    [SerializeField] float maxDampingForce = 50f;// 最大阻尼力

    private Rigidbody2D rb;

    private bool isApplying = false;
    public bool IsApplying
    {
        get { return isApplying; }
        set
        {
            if (isApplying != value)
            {
                changeApplyState(value);
                AudioManager.Instance.PlaySe(value ? "ManetOpen" : "MagnetClose");
            }
            isApplying = value;
        }
    }

    public Action<bool> changeApplyState;


    private void Awake() => rb = GetComponent<Rigidbody2D>();

    private void Update()
    {
        if (!GameManager.Instance.CanInteractNow)
            return;

        if (Input.GetMouseButton(0))
        {
            IsApplying = true;
        }
        else
        {
            IsApplying = false;
        }
    }

    private void FixedUpdate()
    {
        FollowPointer();
        if(isApplying)
        {
            ApplyMagneticForce();
        }
    }

    void ApplyMagneticForce()
    {
        Collider2D[] anchors = Physics2D.OverlapCircleAll(
            transform.position,
            radius,
            anchorLayer
        );

        foreach (Collider2D anchorCollider in anchors)
        {
            MagnetAnchor anchor = anchorCollider.GetComponent<MagnetAnchor>();
            if (!anchor) continue;

            // 计算单个锚点的受力
            Vector2 anchorPos = anchor.WorldPosition;
            Vector2 direction = (Vector2)transform.position - anchorPos;
            float distance = direction.magnitude;

            if (distance <= radius && distance > 0.001f)
            {
                // 计算基础磁力
                float forceMagnitude = Mathf.Clamp(
                    forceFactor / Mathf.Pow(distance, 2),
                    minForce,
                    maxForce
                );
                Vector2 magneticForce = direction.normalized * forceMagnitude;

                // 新增：计算阻尼力（与速度方向相反）
                Vector2 dampingForce = CalculateDampingForce(anchor.parentBody);

                // 合并力并应用
                anchor.parentBody.ApplyCombinedForce(magneticForce + dampingForce);
            }
        }
    }

    Vector2 CalculateDampingForce(MagnetBody body)
    {
        // 获取物体速度
        Vector2 velocity = body.Velocity;

        // 计算阻尼力（方向与速度相反）
        Vector2 dampingDirection = -velocity.normalized;
        float dampingMagnitude = Mathf.Min(
            dampingFactor * velocity.magnitude,
            maxDampingForce
        );

        return dampingDirection * dampingMagnitude;
    }

    void FollowPointer() => rb.MovePosition(Tools.MousePosition);

    void OnDrawGizmos()
    {
        Gizmos.color = gizmoColor;
        Gizmos.DrawWireSphere(transform.position, radius);
    }
}