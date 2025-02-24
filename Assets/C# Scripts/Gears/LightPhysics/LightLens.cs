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

    private State currentState = State.Lock; // ��ʼ״̬Ϊ Lock

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
        // ���������
        if (Input.GetMouseButtonDown(0))
        {
            // �����ǰ״̬�� Lock������Ƿ���������
            if (currentState == State.Lock)
            {
                // ʹ�����߼���Ƿ����˵�ǰ����
                Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
                RaycastHit2D hit = Physics2D.Raycast(ray.origin, ray.direction);

                if (hit.collider != null && hit.collider.gameObject == gameObject)
                {
                    // �л��� Rotate ״̬
                    currentState = State.Rotate;
                }
            }
            // �����ǰ״̬�� Rotate���л��� Lock ״̬
            else if (currentState == State.Rotate)
            {
                currentState = State.Lock;
            }
        }
    }

    private void HandleRotateState()
    {
        // �����ǰ״̬�� Rotate����֡��������� transform.right
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
