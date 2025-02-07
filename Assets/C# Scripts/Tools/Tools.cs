using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Linq;
using DG.Tweening;
using System;

/// <summary>
/// 这是一个工具类
/// 会存储一些常用的功能，并以静态方法存储
/// </summary>
public static class Tools
{
    /// <summary>
    /// 获取鼠标位置的属性
    /// </summary>
    static public Vector2 MousePosition => Camera.main.ScreenToWorldPoint(Input.mousePosition);

    /// <summary>
    /// 获取到玩家
    /// </summary>
    static public GameObject player => GameObject.FindGameObjectWithTag("Player");


    /// <summary>
    /// 获取该名字的层级
    /// </summary>
    /// <param name="name"></param>
    /// <returns></returns>
    static public int GetLayer(string name)
    {
        return 1 << LayerMask.NameToLayer(name);
    }

    /// <summary>
    /// 获得某物体的所有子层级
    /// </summary>
    /// <typeparam name="T"></typeparam>
    /// <param name="transform"></param>
    /// <returns></returns>
    public static T[] GetAllComponentsInChildren<T>(this Transform transform) where T : Component
    {
        T[] results = transform.GetComponentsInChildren<T>();
        for (int i = 0; i < transform.childCount;i++)
            results = results.Concat(GetAllComponentsInChildren<T>(transform.GetChild(i))).ToArray();
        return results;
    }

    /// <summary>
    /// 延迟任务
    /// </summary>
    public static void DelayTask(float delay, Action task)
    {
        DOTween.Sequence()
            .AppendInterval(delay)
            .OnComplete(() => task?.Invoke());
    }
}

