using DG.Tweening;
using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

namespace MyExtensions
{
    public static class DOTweenExtension
    {
        public static Tween DOStartWidth(this LineRenderer lineRenderer, float endValue, float duration, Ease ease = Ease.Linear)
        {
            return DOTween.To(
                () => lineRenderer.startWidth,               // 获取当前值
                x => lineRenderer.startWidth = x,              // 设置新值
                endValue,                                      // 目标值
                duration)                                      // 持续时间
                .SetEase(ease)                                 // 设置缓动类型
                .SetTarget(lineRenderer);                      // 设置目标对象便于控制
        }
    }
}
