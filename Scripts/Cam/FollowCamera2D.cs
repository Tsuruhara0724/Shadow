using System;
using UnityEngine;

namespace Cam
{
    [Flags]
    public enum Direction
    {
        None = 0,
        Horizontal = 1,
        Vertical = 2,
        Both = 3
    }

    public class FollowCamera2D : MonoBehaviour
    {
        public Transform target;  // 目标对象
        public float dampTime = 0.15f; // 平滑时间 
        public Direction followType = Direction.Horizontal; // 跟随类型
        [Range(0.0f,1.0f)]
        public float
            cameraCenterX = 0.5f; // 相机中心点X轴位置（归一化）
        [Range(0.0f,1.0f)]
        public float
            cameraCenterY = 0.5f; // 相机中心点Y轴位置（归一化）
        public Direction boundType = Direction.None; // 边界类型
        public float leftBound = 0; // 左边界
        public float rightBound = 0; // 右边界
        public float upperBound = 0; // 上边界
        public float lowerBound = 0;// 下边界
        public Direction deadZoneType = Direction.None; // 死角类型
        public bool hardDeadZone = false; // 是否使用硬死区
        public float leftDeadBound = 0; // 左死区边界
        public float rightDeadBound = 0; // 右死区边界
        public float upperDeadBound = 0; // 上死区边界
        public float lowerDeadBound = 0; // 下死区边界

        // private
        UnityEngine.Camera camera; // 相机组件
        Vector3 velocity = Vector3.zero; // 速度向量，用于SmoothDamp
        float vertExtent; // 垂直范围
        float horzExtent; // 水平范围
        Vector3 tempVec = Vector3.one; // 临时向量
        bool isBoundHorizontal; // 是否水平边界
        bool isBoundVertical; // 是否垂直边界
        bool isFollowHorizontal; // 是否水平跟随
        bool isFollowVertical; // 是否垂直跟随
        bool isDeadZoneHorizontal; // 是否水平死区
        bool isDeadZoneVertical; // 是否垂直死区
        Vector3 deltaCenterVec; // 相机中心点偏移向量

        void Start()
        {
            camera = GetComponent<UnityEngine.Camera>(); // 获取相机组件
            vertExtent = camera.orthographicSize; // 计算垂直范围
            horzExtent = vertExtent * Screen.width / Screen.height; // 计算水平范围
            deltaCenterVec = camera.ViewportToWorldPoint(new Vector3(0.5f, 0.5f, 0))
                - camera.ViewportToWorldPoint(new Vector3(cameraCenterX, cameraCenterY, 0)); // 计算相机中心偏移向量

            // 初始化各种标志变量
            isFollowHorizontal = (followType & Direction.Horizontal) == Direction.Horizontal;
            isFollowVertical = (followType & Direction.Vertical) == Direction.Vertical;
            isBoundHorizontal = (boundType & Direction.Horizontal) == Direction.Horizontal;
            isBoundVertical = (boundType & Direction.Vertical) == Direction.Vertical;
            isDeadZoneHorizontal = ((deadZoneType & Direction.Horizontal) == Direction.Horizontal) && isFollowHorizontal;
            isDeadZoneVertical = ((deadZoneType & Direction.Vertical) == Direction.Vertical) && isFollowVertical;
            tempVec = Vector3.one;
        }

        void LateUpdate()
        {
            if (target)
            {
                Vector3 delta = target.position - camera.ViewportToWorldPoint(new Vector3(cameraCenterX, cameraCenterY, 0));
                if (!isFollowHorizontal)
                { delta.x = 0; }
                if (!isFollowVertical)
                { delta.y = 0; }
                Vector3 destination = transform.position + delta;
                if (!hardDeadZone)
                { tempVec = Vector3.SmoothDamp(transform.position, destination, ref velocity, dampTime);}
                else
                {
                    tempVec.Set(transform.position.x, transform.position.y, transform.position.z);
                }
                if (isDeadZoneHorizontal)
                {
                    if (delta.x > rightDeadBound)
                    {
                        tempVec.x = target.position.x - rightDeadBound + deltaCenterVec.x;
                    }
                    if (delta.x < -leftDeadBound)
                    {
                        tempVec.x = target.position.x + leftDeadBound + deltaCenterVec.x;
                    }
                }
                if (isDeadZoneVertical)
                {
                    if (delta.y > upperDeadBound)
                    {
                        tempVec.y = target.position.y - upperDeadBound + deltaCenterVec.y;
                    }
                    if (delta.y < -lowerDeadBound)
                    {
                        tempVec.y = target.position.y + lowerDeadBound + deltaCenterVec.y;
                    }
                }
                if (isBoundHorizontal)
                {
                    tempVec.x = Mathf.Clamp(tempVec.x, leftBound + horzExtent, rightBound - horzExtent);
                }
                if (isBoundVertical)
                {
                    tempVec.y = Mathf.Clamp(tempVec.y, lowerBound + vertExtent, upperBound - vertExtent);
                }
                tempVec.z = transform.position.z;
                transform.position = tempVec;
            }
        }
    }

}