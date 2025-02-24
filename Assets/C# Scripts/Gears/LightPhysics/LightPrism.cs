using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LightPrism : LightPhysics
{
    private LightLine[] lines;
    [SerializeField] private float refractAngle = 60f;

    private void Awake()
    {
        lines = GetComponentsInChildren<LightLine>();
        foreach (LightLine line in lines)
            line.SetActivate(false);
    }

    private void Update()
    {
        Effect();
    }

    private void Effect()
    {
        if (!IsHit)
            return;

        var incomingLine = lightLines[0];

        lines[0].SetParameters(transform.position, Quaternion.Euler(0, 0, refractAngle) * incomingLine.Dir);
        lines[1].SetParameters(transform.position, Quaternion.Euler(0, 0, -refractAngle) * incomingLine.Dir);
    }

    protected override void OnHit()
    {
        foreach (LightLine line in lines)
        {
            line.SetActivate(true);
        }
    }

    protected override void OnLeave()
    {
        foreach (LightLine line in lines)
        {
            line.SetActivate(false);
        }
    }
}
