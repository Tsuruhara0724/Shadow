using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ZoomOut : MonoBehaviour
{

    // 私有变量，用于存储相机组件
    private Camera _cam;

    // 公共变量，用于控制相机是否缩小视野
    public bool ZoomOutCamera = false;

    // 公共变量，用于存储相机的初始视野和缩小后的视野
    public float InitialFOV;
    public float ZoomoutFOV;

    // 公共变量，用于控制视野过渡的平滑时间
    public float DampTime;

    // 私有变量，用于存储当前视野值
    private float _current;

    // 在脚本启动时调用
    void Start()
    {
        // 获取相机组件
        _cam = GetComponent<Camera>();

        // 存储相机的初始视野值
        InitialFOV = _cam.fieldOfView;

        // 初始化当前视野值为初始视野值
        _current = InitialFOV;
    }

    // 在每帧更新时调用
    void Update()
    {
        // 根据ZoomOutCamera布尔值决定目标视野值
        var target = ZoomOutCamera ? ZoomoutFOV : InitialFOV;

        // 使用Mathf.SmoothDamp平滑过渡视野值
        _cam.fieldOfView = Mathf.SmoothDamp(_cam.fieldOfView, target, ref _current, DampTime);
    }
}
