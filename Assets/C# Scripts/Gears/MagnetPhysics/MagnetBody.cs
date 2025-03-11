using UnityEngine;

[RequireComponent(typeof(Rigidbody2D))]
public class MagnetBody : MonoBehaviour
{
    public Vector2 Velocity => rb.velocity;

    public Vector2 magnetMultiplier = new Vector2(1,1); //水平方向和竖直方向对磁力的相应程度

    private MagnetAnchor[] anchors;    // 所有锚点
    private Rigidbody2D rb;           // 本体刚体

    protected virtual void Awake()
    {
        anchors = GetComponentsInChildren<MagnetAnchor>();
        rb = GetComponent<Rigidbody2D>();
    }

    // 应用总力到本体
    public void ApplyCombinedForce(Vector2 totalForce)
    {
        var x = totalForce.x * magnetMultiplier.x;
        var y = totalForce.y * magnetMultiplier.y;

        rb.AddForce(new Vector2(x,y));
    }
}

