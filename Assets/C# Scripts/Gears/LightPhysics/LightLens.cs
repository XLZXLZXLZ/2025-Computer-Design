using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;

public class LightLens : LightPhysics
{
    private LightLine line;
    private Vector3 originalScale;

    private enum State
    {
        Lock,
        Rotate
    }

    private State currentState = State.Lock;

    private void Awake()
    {
        line = GetComponentInChildren<LightLine>();
        line.SetActivate(false);
        originalScale = transform.localScale;
    }

    private void Update()
    {
        HandleMouseInput();
        Effect();
    }

    private void HandleMouseInput()
    {
        if (!GameManager.Instance.CanInteractNow)
            return;

        if (Input.GetMouseButtonDown(0))
        {
            Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
            RaycastHit2D hit = Physics2D.Raycast(ray.origin, ray.direction);

            if (hit.collider != null && (hit.collider.gameObject == gameObject || ((hit.collider.transform.parent != null) && (hit.collider.transform.parent.gameObject == gameObject))))
            {
                currentState = State.Rotate;
                PlaySelectAnimation();
                AudioManager.Instance.PlaySe("LensClick");
            }
            else
            {
                if (currentState == State.Rotate)
                {
                    RotateTowardsMouse();
                    currentState = State.Lock;
                }
            }
        }
    }

    private void PlaySelectAnimation()
    {
        transform.DOPunchScale(originalScale * 0.2f, 0.5f, 2, 0.5f);
    }

    private void RotateTowardsMouse()
    {
        Vector2 mousePosition = Camera.main.ScreenToWorldPoint(Input.mousePosition);
        Vector2 direction = mousePosition - (Vector2)transform.position;
        float targetAngle = Mathf.Atan2(direction.y, direction.x) * Mathf.Rad2Deg;

        transform.DORotate(new Vector3(0, 0, targetAngle), 0.5f)
            .SetEase(Ease.OutQuad)
            .OnComplete(() => transform.right = direction.normalized);
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