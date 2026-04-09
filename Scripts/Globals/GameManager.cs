using System;
using System.Collections;
using System.Collections.Generic;
using Cam;
using UnityEngine;

using System.Collections;
using UnityEngine;
using Mirror;

public class GameManager : NetworkBehaviour
{
    // 玩家光源和阴影对象的引用
    public GameObject PlayerLight; // 光源玩家对象
    public GameObject PlayerShadow; // 阴影玩家对象

    // 关卡段数组
    public LevelSegment[] Segments; // 所有关卡段数组
    public LevelSegment CurrentSegment; // 当前活动的关卡段

    private int aindex = 0;

    // 初始化
    void Start()
    {
        Time.timeScale = 0;
        // 在事件中心注册 GameManager
        Hub.Register(this);

        // 查找和分配玩家对象
        PlayerLight = GameObject.Find("PlayerLight");
        PlayerShadow = GameObject.Find("PlayerShadow");

        // 查找和组织关卡段
        var levelSegmentList = new List<LevelSegment>();
        foreach (Transform child in GameObject.Find("Segments").transform)
        {
            var segment = child.gameObject.GetComponent<LevelSegment>();
            if (!child.gameObject.CompareTag("ignore")) // 如果标签不是 "ignore"
            {
                levelSegmentList.Add(segment);
            }
        }
        Segments = levelSegmentList.ToArray(); // 转换列表为数组

        //// 在奇偶关卡段中翻转桌子
        //for (int i = 0; i < Segments.Length; i++)
        //{
        //    var table = Segments[i].transform.Find("Table");
        //    var scaleX = table.localScale.x * (i % 2 == 0 ? -1 : 1); // 根据奇偶索引进行翻转
        //    table.localScale = new Vector3(scaleX, table.localScale.y, table.localScale.z);
        //}

        // 如果有关卡段
        if (Segments.Length != 0)
        {
            CurrentSegment = Segments[0]; // 设置初始关卡段
            Reset(false, true); // 重置游戏状态，不影响摄像机（初始重置）
        }

        // 播放默认背景音乐
        transform.GetComponent<AudioControl>().PlayDefaultMusic();
    }

    // 每帧更新
    private void Update()
    {
        // 检测重置输入
        if (Input.GetButtonDown("Reset"))  // Escape键
        {
            BackToMain();
        }

        if (PlayerShadow.transform.position.x < CurrentSegment.InitialPlayerPosition.x + CurrentSegment.LevelOffset.x - 2) { Reset(); }
    }

    // 重置游戏状态
    public void Reset(bool cameraToo = false, bool initial = false)
    {
        // 确保当前关卡段有效
        if (!CurrentSegment)
        {
            Debug.LogWarning("Reset without CurrentSegment called");
            return;
        }

        aindex = Array.FindIndex(Segments, (e) => e == CurrentSegment);

        // 设置影子和溅起的颜色
        PlayerShadow.GetComponent<SpriteRenderer>().color = CurrentSegment.ShadowColor;
        //PlayerShadow.GetComponentInChildren<Material>().color = CurrentSegment.ShadowColor;
        //CurrentSegment.GetComponentInChildren<EndOfLevel>().GetComponent<SpriteRenderer>().color = CurrentSegment.ShadowColor;


        // 重置玩家位置

        PlayerLight.transform.position = CurrentSegment.InitialSunPosition + CurrentSegment.LevelOffset;
        PlayerShadow.transform.position = CurrentSegment.InitialPlayerPosition + CurrentSegment.LevelOffset + Vector3.up * 0.01f;

        // 如果需要，执行与摄像机相关的操作
        if (cameraToo)
        {
            StartCoroutine(SlowCamera());
            Hub.Get<AudioControl>().PlaySound("amb_book_flip_3"); // 播放摄像机动画的音效
        }
        else if (!initial) // 播放普通重置的音效
        {
            Hub.Get<AudioControl>().PlaySound("player_reset_1");
        }
    }

    // 临时减慢摄像机运动
    private IEnumerator SlowCamera()
    {
        var followScript = Camera.main.gameObject.GetComponent<FollowCamera2D>(); // 获取摄像机跟随脚本
        var oldDamp = followScript.dampTime; // 保存原始阻尼时间
        var tempDamp = oldDamp * 5; // 临时增加阻尼时间
        followScript.dampTime = tempDamp; // 应用新的阻尼时间

        var zoomOut = Camera.main.gameObject.GetComponent<ZoomOut>(); // 获取摄像机缩放脚本
        zoomOut.DampTime = tempDamp; // 设置缩放阻尼时间
        zoomOut.ZoomOutCamera = true; // 触发缩放效果
        yield return new WaitForSeconds(tempDamp); // 等待阻尼时间
        zoomOut.ZoomOutCamera = false; // 结束缩放效果
        yield return new WaitForSeconds(tempDamp); // 等待额外的阻尼时间
        followScript.dampTime = oldDamp;
    }
    [Command(requiresAuthority = false)]
    public void EndOfLevelReached()
    {

        EndOfLevelReached1(aindex);


    }
    [ClientRpc]
    // 关卡结束触发方法
    public void EndOfLevelReached1(int index)
    {
        // 获取当前关卡段索引
        if (index == Segments.Length - 1) // 如果是最后一个关卡段
        {
            Debug.LogWarning("Last Segment Overcome!"); // 输出警告信息
            BackToMain(); // 返回主界面
        }
        else // 如果不是最后一个关卡段
        {
            CurrentSegment = Segments[index + 1]; // 设置下一个关卡段为当前关卡段
            Reset(true); // 重置游戏状态
        }
    }


    // 通关界面方法
    public void GameClear()
    {

    }

    // 返回主界面方法
    public void BackToMain()
    {
        CurrentSegment = Segments[0]; // 设置当前关卡段为第一个关卡段
        Reset(true); // 重置游戏状态
    }
}