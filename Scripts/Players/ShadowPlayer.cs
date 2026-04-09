using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Assertions.Comparers;

public class ShadowPlayer : MonoBehaviour
{
    // 公共变量
    public float GravityScale = 1;  // 重力缩放比例
    public bool DebugStuff = false; // 是否启用调试信息

    public float Speed = 3; // 水平移动速度

    public bool Sticked = false; // 是否粘在墙上
    public bool PreviousLit = false; // 前一帧是否在光照下

    private static float JumpTreshold = 0.5f; // 跳跃阈值

    // 私有变量
    private TestIfLit _testIfLid; // 测试是否在光照下的组件
    private SpriteRenderer _spriteRenderer; // 精灵渲染器组件

    // 初始化
    void Start()
    {
        _testIfLid = GetComponent<TestIfLit>(); // 获取TestIfLit组件
        _spriteRenderer = GetComponent<SpriteRenderer>(); // 获取SpriteRenderer组件
    }

    // 在每帧更新后调用
    void LateUpdate()
    {
        // 计算重力向量
        Vector3 gravity = Vector3.forward * Physics.gravity.magnitude * GravityScale;
        // 获取输入
        float jumpAxis = Input.GetAxis("ShadowJump");
        float speedX = Input.GetAxis("ShadowHorizontal");
        // 判断是否跳跃
        bool Jump = jumpAxis > JumpTreshold;
        // 检查是否在光照下
        bool lit = _testIfLid.Lit;
        // 如果前一帧在光照下而当前帧不在，则粘在墙上
        if (PreviousLit && !lit)
        {
            Sticked = true;
        }
        // 如果在光照下
        if (lit)
        {
            // 角色受重力影响下降，并根据水平输入移动
            transform.position += gravity * Time.deltaTime;
            transform.position += Vector3.right * Speed * speedX * Time.deltaTime;
        }
        // 如果在阴影中并且粘在墙上且检测到跳跃输入
        else if (Sticked && Jump)
        {
            Sticked = false; // 解除粘贴状态
        }
        // 如果启用了调试信息，输出相关调试信息
        if (DebugStuff)
        {
            Debug.Log($"Jump Axis: " + jumpAxis);
            Debug.Log($"ShadowHorizontal: " + speedX);
        }
        // 更新前一帧的光照状态
        PreviousLit = lit;
    }
}