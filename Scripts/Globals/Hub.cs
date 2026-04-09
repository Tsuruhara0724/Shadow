using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Hub : MonoBehaviour
{
    private static IDictionary<Type, object> _instances; // 存储类型和实例的字典

    public static void Init()
    {
        _instances = new Dictionary<Type, object>(); // 初始化实例字典
    }

    void Awake()
    {
        Init(); // 在 Awake 方法中初始化实例字典
        if (Register<EventHub>() == null) // 注册 EventHub 实例
        {
            Register(new EventHub());
        }
    }

    public static T Get<T>()
    {
        return (T)_instances[typeof(T)]; // 获取特定类型的实例
    }

    public static void Register<T>(T obj)
    {
        _instances[typeof(T)] = obj; // 注册特定类型的实例
    }

    public static T Register<T>() where T : UnityEngine.Object
    {
        var obj = FindObjectOfType<T>(); // 查找场景中的指定类型的对象
        if (obj != null)
            _instances[typeof(T)] = obj; // 注册找到的对象实例

        return obj; // 返回找到的对象实例
    }

}
