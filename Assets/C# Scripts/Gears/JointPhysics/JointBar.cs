using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// ¹Ø½ÚÄ¾Ìõ
/// </summary>
public class JointBar : MonoBehaviour
{
    private Dictionary<InGameJoint, HingeJoint2D> jointDict = new();

    private void Awake()
    {
        var inGameJoints = GetComponentsInChildren<InGameJoint>();
        foreach (var j in inGameJoints)
        {
            var newJoint = gameObject.AddComponent<HingeJoint2D>();
            newJoint.anchor = j.transform.localPosition;
            jointDict[j] = newJoint;
            j.onRelease += OnRelease;
        }
    }

    private void OnRelease(InGameJoint joint)
    {
        jointDict[joint].enabled = false;
    }
}
