using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Gear : MonoBehaviour
{
    [SerializeField]
    private Switch target;

    protected bool IsOn => target.IsOn;

    protected virtual void Awake()
    {
        target.SwitchOn += SwitchOn;
        target.SwitchOff += SwitchOff;
    }

    protected virtual void SwitchOn()
    {

    }

    protected virtual void SwitchOff()
    {

    }

    protected virtual void OnDrawGizmos()
    {
        if(target == null) return;

        Gizmos.color = Color.red;

        Gizmos.DrawLine(transform.position, target.transform.position);
        Gizmos.DrawWireSphere(target.transform.position, 0.5f);
    }
}
