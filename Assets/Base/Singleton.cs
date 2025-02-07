using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

/// <summary>
/// 单例基类,只需继承该基类即可变为单例,用法同正常单例
/// </summary>
/// <typeparam name="T">子类名称</typeparam>
public class Singleton<T> : MonoBehaviour where T : Singleton<T>
{
    protected virtual bool IsDonDestroyOnLoad => false;

    private static T instance;
    public static T Instance //单例
    {
        get
        {
            if (instance == null || instance.IsUnityNull())
            {
                instance = FindObjectOfType<T>();
                if (instance == null || instance.IsUnityNull())
                {
                    GameObject go = new GameObject(typeof(T).Name);
                    instance = go.AddComponent<T>();
                }
            }
            return instance;
        }
    }

    protected virtual void Awake()
    {
        if (instance == null || instance.IsUnityNull())
            instance = this as T;
        else if (instance != this)
        {
            Destroy(gameObject);
        }

        if (IsDonDestroyOnLoad)
            DontDestroyOnLoad(gameObject);
    }
}