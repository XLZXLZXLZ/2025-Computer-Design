using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LightLens : LightPhysics
{
    private LightLine line;

    private enum State
    {
        Lock,
        Rotate
    }

    private State currentState = State.Lock; // 初始状态为 Lock

    private void Awake()
    {
        line = GetComponentInChildren<LightLine>();
        line.SetActivate(false);
    }

    private void Update()
    {
        HandleMouseInput();
        HandleRotateState();
        Effect();
    }

    private void HandleMouseInput()
    {
        // 检测左键点击
        if (Input.GetMouseButtonDown(0))
        {
            // 如果当前状态是 Lock，检查是否点击了物体
            if (currentState == State.Lock)
            {
                // 使用射线检测是否点击了当前物体
                Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
                RaycastHit2D hit = Physics2D.Raycast(ray.origin, ray.direction);

                if (hit.collider != null && hit.collider.gameObject == gameObject)
                {
                    // 切换到 Rotate 状态
                    currentState = State.Rotate;
                }
            }
            // 如果当前状态是 Rotate，切换到 Lock 状态
            else if (currentState == State.Rotate)
            {
                currentState = State.Lock;
            }
        }
    }

    private void HandleRotateState()
    {
        // 如果当前状态是 Rotate，逐帧更新物体的 transform.right
        if (currentState == State.Rotate)
        {
            Vector2 mousePosition = Camera.main.ScreenToWorldPoint(Input.mousePosition);
            transform.right = mousePosition - (Vector2)transform.position;
        }
    }

    private void Effect()
    {
        line.SetParameters(transform.position, transform.right);
    }

    protected override void OnHit()
    {
        line.SetActivate(true);
    }

    protected override void OnLeave()
    {
        line.SetActivate(false);
    }

    private void OnDrawGizmosSelected()
    {
        Gizmos.color = Color.yellow;
        Gizmos.DrawRay(transform.position, transform.right * 2f);
    }
}
