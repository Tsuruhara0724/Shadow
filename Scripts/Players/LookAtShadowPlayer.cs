using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class LookAtShadowPlayer : MonoBehaviour
{
    // 公共变量，用于在 Unity 编辑器中指定要跟随的 ShadowPlayer 对象
    public GameObject ShadowPlayer;

    // Use this for initialization
    void Start()
    {
        // 初始化方法，在游戏开始时调用。此处未进行任何初始化操作。
    }

    // Update is called once per frame
    void Update()
    {
        // 每帧调用，使当前游戏对象始终面向 ShadowPlayer
        transform.LookAt(ShadowPlayer.transform);
    }
}
