using Mirror;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class NaiveLightMovement : NetworkBehaviour
{
    // 公共变量
    public float Speed = 3;          // 移动速度
    public float BoundTop = 0;       // 上边界
    public float BoundBottom = 0;    // 下边界
    //public float BoundLeft = 0;      // 左边界
    //public float BoundRight = 0;     // 右边界
    public float BoundFront = 0;    //前边界
    public float BoundBack = 0;    //后边界
    public bool DebugSpeed = false;  // 是否输出调试信息

    public int Inverted = 1;         // 方向反转标志

    void Start() 
    {
        
    }

    void Update()
    {
        // 联机时移动功能需要先上传服务器再由服务器通知客户端，因此需要一个位置信息存放用户输入后变化位置，服务器下放后再进行更改
        Vector3 newPos = transform.position;
        // 获取移动输入，处理死区
        var movement = DeadZoned();

        //判断是否接收 Network Identity -> 判断是由哪个客户端控制
        if (isOwned)
        {
            // 检查是否需要反转方向
            if (Input.GetAxis("LightInvert") > 0.5)
            {
                Inverted *= -1;
            }
            // 如果反转标志为1，则反转Y轴方向
            if (Inverted == 1)
            {
                movement = new Vector2(movement.x, movement.y * -1);
            }
            // 如果启用了调试信息，输出当前水平和垂直输入轴的值
            if (DebugSpeed)
            {
                Debug.Log(Input.GetAxis("LightHorizontal"));
                Debug.Log(Input.GetAxis("LightVertical"));
            }
            // 获取Z轴的速度，根据按键'Q'和'E'确定速度值
            var speedZ = 0f;
            if (Input.GetKey(KeyCode.Q))
            {
                speedZ = Speed;
            }
            else if (Input.GetKey(KeyCode.E))
            {
                speedZ = -Speed;
            }
            // 根据按键确定水平和垂直方向的移动值
            if (Input.GetKey(KeyCode.A))
            {
                movement.x = -1;
            }
            else if (Input.GetKey(KeyCode.D))
            {
                movement.x = 1;
            }
            if (Input.GetKey(KeyCode.W))
            {
                movement.y = 1;
            }
            else if (Input.GetKey(KeyCode.S))
            {
                movement.y = -1;
            }
            // 根据计算的移动值更新光源的位置（上传服务器前）
            newPos = transform.position +
                new Vector3(movement.x * Time.deltaTime * Speed, movement.y * Time.deltaTime * Speed, speedZ * Time.deltaTime);

        }

        // 根据计算的移动值更新光源的位置（服务器下发更新）
        transform.position = newPos;

        // 检查上下边界限制，确保光源不会超出预定义的边界
        if (transform.position.y < BoundBottom)
        {
            transform.position = new Vector3(transform.position.x, BoundBottom, transform.position.z);
        }
        if (transform.position.y > BoundTop)
        {
            transform.position = new Vector3(transform.position.x, BoundTop, transform.position.z);
        }

        // 检查前后边界限制，0~10
        if(transform.position.z < BoundBack)
        {
            transform.position = new Vector3(transform.position.x, transform.position.y, BoundBack);
        }
        if (transform.position.z > BoundFront)
        {
            transform.position = new Vector3(transform.position.x, transform.position.y, BoundFront);
        }

        // 检查场景并确定左右边界 -15~15
        //场景0
        if (Hub.Get<GameManager>().CurrentSegment == Hub.Get<GameManager>().Segments[0])
        {
            if(transform.position.x < -15f)
            {
                transform.position = new Vector3(-15f, transform.position.y, transform.position.z);
            }
            if (transform.position.x > 15f)
            {
                transform.position = new Vector3(15f, transform.position.y, transform.position.z);
            }
        }
        //场景1
        if (Hub.Get<GameManager>().CurrentSegment == Hub.Get<GameManager>().Segments[1])
        {
            Debug.Log("场景一");
            if (transform.position.x < 25f)
            {
                transform.position = new Vector3(25f, transform.position.y, transform.position.z);
            }
            if (transform.position.x > 55f)
            {
                transform.position = new Vector3(55f, transform.position.y, transform.position.z);
            }
        }
        //场景2
        if (Hub.Get<GameManager>().CurrentSegment == Hub.Get<GameManager>().Segments[2])
        {
            if (transform.position.x < 65f)
            {
                transform.position = new Vector3(65f, transform.position.y, transform.position.z);
            }
            if (transform.position.x > 95f)
            {
                transform.position = new Vector3(95f, transform.position.y, transform.position.z);
            }
        }
        //场景3
        if (Hub.Get<GameManager>().CurrentSegment == Hub.Get<GameManager>().Segments[3])
        {
            if (transform.position.x < 105f)
            {
                transform.position = new Vector3(105f, transform.position.y, transform.position.z);
            }
            if (transform.position.x > 135f)
            {
                transform.position = new Vector3(135f, transform.position.y, transform.position.z);
            }
        }
        //场景4
        if (Hub.Get<GameManager>().CurrentSegment == Hub.Get<GameManager>().Segments[4])
        {
            if (transform.position.x < 145f)
            {
                transform.position = new Vector3(145f, transform.position.y, transform.position.z);
            }
            if (transform.position.x > 175f)
            {
                transform.position = new Vector3(175f, transform.position.y, transform.position.z);
            }
        }
    }

    // 处理死区的函数
    private Vector2 DeadZoned()
    {
        float deadzone = 0.25f;
        Vector2 stickInput = new Vector2(Input.GetAxis("LightHorizontal"), Input.GetAxis("LightVertical"));
        if (stickInput.magnitude < deadzone)
            stickInput = Vector2.zero;
        else
            stickInput = stickInput.normalized * ((stickInput.magnitude - deadzone) / (1 - deadzone));
        return stickInput;
    }

    // Command是由客户端向服务器发送请求
    [Command(requiresAuthority = false)]
    public void CmdMove(Vector3 newPosition)
    {
        CmdMoveObject(newPosition);
    }

    // ClientRpc是由服务器向所有客户端通知
    [ClientRpc]
    public void CmdMoveObject(Vector3 newPosition)
    {
        transform.position = newPosition;


        // 检查上下边界限制，确保光源不会超出预定义的边界
        if (transform.position.y < BoundBottom)
        {
            transform.position = new Vector3(transform.position.x, BoundBottom, transform.position.z);
            Debug.Log("BoundBottom");
        }
        if (transform.position.y > BoundTop)
        {
            transform.position = new Vector3(transform.position.x, BoundTop, transform.position.z);
        }

        // 检查前后边界限制，0~10
        if (transform.position.z < BoundBack)
        {
            transform.position = new Vector3(transform.position.x, transform.position.y, BoundBack);
        }
        if (transform.position.z > BoundFront)
        {
            transform.position = new Vector3(transform.position.x, transform.position.y, BoundFront);
        }

        // 检查场景并确定左右边界 -15~15
        //场景0
        if (Hub.Get<GameManager>().CurrentSegment == Hub.Get<GameManager>().Segments[0])
        {
            if (transform.position.x < -15f)
            {
                transform.position = new Vector3(-15f, transform.position.y, transform.position.z);
            }
            if (transform.position.x > 15f)
            {
                transform.position = new Vector3(15f, transform.position.y, transform.position.z);
            }
        }
        //场景1
        if (Hub.Get<GameManager>().CurrentSegment == Hub.Get<GameManager>().Segments[1])
        {
            Debug.Log("场景一");
            if (transform.position.x < 25f)
            {
                transform.position = new Vector3(25f, transform.position.y, transform.position.z);
            }
            if (transform.position.x > 55f)
            {
                transform.position = new Vector3(55f, transform.position.y, transform.position.z);
            }
        }
        //场景2
        if (Hub.Get<GameManager>().CurrentSegment == Hub.Get<GameManager>().Segments[2])
        {
            if (transform.position.x < 65f)
            {
                transform.position = new Vector3(65f, transform.position.y, transform.position.z);
            }
            if (transform.position.x > 95f)
            {
                transform.position = new Vector3(95f, transform.position.y, transform.position.z);
            }
        }
        //场景3
        if (Hub.Get<GameManager>().CurrentSegment == Hub.Get<GameManager>().Segments[3])
        {
            if (transform.position.x < 105f)
            {
                transform.position = new Vector3(105f, transform.position.y, transform.position.z);
            }
            if (transform.position.x > 135f)
            {
                transform.position = new Vector3(135f, transform.position.y, transform.position.z);
            }
        }
        //场景4
        if (Hub.Get<GameManager>().CurrentSegment == Hub.Get<GameManager>().Segments[4])
        {
            if (transform.position.x < 145f)
            {
                transform.position = new Vector3(145f, transform.position.y, transform.position.z);
            }
            if (transform.position.x > 175f)
            {
                transform.position = new Vector3(175f, transform.position.y, transform.position.z);
            }
        }
    }

}

