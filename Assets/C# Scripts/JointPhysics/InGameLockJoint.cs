using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class InGameLockJoint : InGameJoint
{
    private Color showColor = Color.yellow;
    private void OnDrawGizmosSelected()
    {
        Gizmos.color = showColor;
        Gizmos.DrawWireSphere(transform.position, 0.5f);
    }

    public void Release()
    {
        onRelease?.Invoke(this);
    }

}
