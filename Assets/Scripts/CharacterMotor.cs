using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

public class CharacterMotor : MonoBehaviour
{
    const int INVALID = -1;
    const int RIGISING_STATE = 1;//上升状态常量
    const int FALLING_STATE = 2;//下落状态常量
    const int MOVING_STATE = 4;//移动状态常量
    const int ON_GROUND_STATE = 8;//地面状态常量

    int mInternalState;
    Vector3? mLastPosition;//可空类型存放上一帧坐标
    RaycastHit mGroundRaycastHit;//当前帧的投射信息

    public LayerMask groundLayerMask;
    public bool isDebug;
    public Transform[] footPoints;//地面检测点

    public bool IsMoving { get { return (mInternalState & MOVING_STATE) == MOVING_STATE; } }//是否处于移动
    public bool IsRising { get { return (mInternalState & RIGISING_STATE) == RIGISING_STATE; } }//是否处于上升
    public bool IsFalling { get { return (mInternalState & FALLING_STATE) == FALLING_STATE; } }//是否处于下降
    public bool IsOnGround { get { return (mInternalState & ON_GROUND_STATE) == ON_GROUND_STATE; } }//是否在地面
    public RaycastHit GroundRaycastHit { get { return mGroundRaycastHit; } }
    public event Action OnGroundRaycastHited;//碰到地面的回调


    public void UpdateGroundDetection()//更新地面检测
    {
        const float GROUND_DETECTE_LEN = 0.5f;//地面线段检测长度
        var downAxis = Physics.gravity.normalized;//垂直向下方向
        mInternalState = mInternalState & (~ON_GROUND_STATE);//重置地面状态
        for (int i = 0; i < footPoints.Length; i++)//footPoints是脚部检测点
        {
            var raycastHit = default(RaycastHit);
            Debug.DrawRay(footPoints[i].position, downAxis,Color.red);
            if (Physics.Raycast(new Ray(footPoints[i].position, downAxis), out raycastHit, GROUND_DETECTE_LEN, groundLayerMask))
            {
                mInternalState |= ON_GROUND_STATE;//设置地面状态
                mGroundRaycastHit = raycastHit;//缓存hit信息
                if (OnGroundRaycastHited != null)//触碰地面回调
                    OnGroundRaycastHited();
                break;
            }
        }
    }

    public void UpdateMovingStateDetect()
    {
        const float MOVING_EPS = 0.0001f;
        var upAxis = Physics.gravity.normalized;//垂直向上方向
        mInternalState = mInternalState & (~MOVING_STATE);
        mInternalState = mInternalState & (~RIGISING_STATE);
        mInternalState = mInternalState & (~FALLING_STATE);//状态重置
        var velocity = transform.position - mLastPosition.GetValueOrDefault(transform.position);
        if (velocity.magnitude > MOVING_EPS)//移动速率大于误差，说明在移动
        {
            mInternalState |= MOVING_STATE;
            var pitchValue = Vector3.Dot(velocity, upAxis);//垂直方向投影
            if (pitchValue > 0) mInternalState |= RIGISING_STATE;//大于0是上升状态
            else if (pitchValue < 0) mInternalState |= FALLING_STATE;//小于0是下降状态
        }
        mLastPosition = transform.position;//缓存上次坐标
    }

    void Update()
    {
        UpdateGroundDetection();
        UpdateMovingStateDetect();
        Debug.Log(IsOnGround);
    }
}
