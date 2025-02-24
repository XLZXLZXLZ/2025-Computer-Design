using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

//关节类，将关节放在JointBar下，在游戏开始时在对应位置生成关节
public class InGameJoint : MonoBehaviour
{
    public Action<InGameJoint> onRelease; //当该结点被解除时
}
