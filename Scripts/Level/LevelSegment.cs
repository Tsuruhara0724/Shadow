using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[Serializable]

public class LevelSegment : MonoBehaviour
{
    // 关卡段的偏移量，默认为原点
    public Vector3 LevelOffset = Vector3.zero;

    // 关卡段的阴影颜色，默认为 (67, 70, 77, 255)
    public Color ShadowColor = new Color(67, 70, 77, 255);

    // 太阳（光源）的初始位置，默认为 (0, 4, 0.6)
    public Vector3 InitialSunPosition = new Vector3(0, 4, 0.6f);

    // 摄像机的初始位置，默认为 (0, 5, 0.8)
    public Vector3 InitialCameraPosition = new Vector3(0, 5, 0.8f);

    // 玩家的初始位置，默认为 (0, 0, -5)
    public Vector3 InitialPlayerPosition = new Vector3(0, 0, -5);

    // 在对象被唤醒时调用的方法
    void Awake()
    {
        // 将关卡段的位置赋值给 LevelOffset 以便在关卡段移动时跟踪偏移量
        LevelOffset = transform.position;
    }

}
