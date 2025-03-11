using UnityEngine;

[RequireComponent(typeof(Collider2D))]
public class MagnetAnchor : MonoBehaviour
{
    [Header("Anchor Settings")]
    public Vector2 localOffset = Vector2.zero; // 相对于父物体的位置偏移
    public float debugRadius = 0.1f;           // 调试显示半径

    public MagnetBody parentBody;             // 所属的磁体

    private void Awake()
    {
        parentBody = GetComponentInParent<MagnetBody>();
        GetComponent<Collider2D>().isTrigger = true;
    }

    public Vector2 WorldPosition =>
        (Vector2)parentBody.transform.TransformPoint(localOffset);

    void OnDrawGizmos()
    {
        Gizmos.color = Color.cyan;
        Gizmos.DrawWireSphere(transform.position, debugRadius);
    }
}