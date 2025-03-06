using System.Collections;
using System.Collections.Generic;
using UnityEngine;

using DG.Tweening;

public class RotateBar : Gear
{
    public enum TransformType
    {
        Loop,
        PingPong
    }

    public Vector3[] rotateStates;
    public TransformType transformType;

    [SerializeField]
    private int currentIndex;
    [SerializeField]
    private float moveTime = 1f;

    private int direction = 1; // 1表示正向，-1表示反向

    protected override void Awake()
    {
        base.Awake();
        // 初始化时设置到当前索引对应的角度
        if (rotateStates != null && rotateStates.Length > 0)
        {
            transform.eulerAngles = rotateStates[currentIndex];
        }
    }

    private void Rotate()
    {
        if (rotateStates == null || rotateStates.Length == 0) return;

        // 更新当前索引
        if (transformType == TransformType.Loop)
        {
            currentIndex = (currentIndex + 1) % rotateStates.Length;
        }
        else // PingPong模式
        {
            currentIndex += direction;
            // 边界检测并反转方向
            if (currentIndex >= rotateStates.Length || currentIndex < 0)
            {
                direction *= -1;
                currentIndex += 2 * direction;
            }
        }

        // 使用DOTween旋转到目标角度
        transform.DORotate(rotateStates[currentIndex], moveTime).SetEase(Ease.InQuad);
    }

    protected override void SwitchOn()
    {
        base.SwitchOn();
        Rotate();
    }

    protected override void SwitchOff()
    {
        base.SwitchOff();
        Rotate();
    }
}