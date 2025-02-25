using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LightSource : MonoBehaviour
{
    private LightLine line;

    private void Awake()
    {
        line = GetComponentInChildren<LightLine>();
        line.SetActivate(true);
        line.SetParameters(transform.position, transform.right);
    }

    private void OnDrawGizmos()
    {
        Gizmos.color = Color.yellow;
        Gizmos.DrawRay(transform.position, transform.right * 100);
        Gizmos.DrawWireSphere(transform.position, 0.5f);
    }
}
