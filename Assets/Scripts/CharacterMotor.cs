using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

public class CharacterMotor : MonoBehaviour
{
    const int INVALID = -1;
    const int RIGISING_STATE = 1;//����״̬����
    const int FALLING_STATE = 2;//����״̬����
    const int MOVING_STATE = 4;//�ƶ�״̬����
    const int ON_GROUND_STATE = 8;//����״̬����

    int mInternalState;
    Vector3? mLastPosition;//�ɿ����ʹ����һ֡����
    RaycastHit mGroundRaycastHit;//��ǰ֡��Ͷ����Ϣ

    public LayerMask groundLayerMask;
    public bool isDebug;
    public Transform[] footPoints;//�������

    public bool IsMoving { get { return (mInternalState & MOVING_STATE) == MOVING_STATE; } }//�Ƿ����ƶ�
    public bool IsRising { get { return (mInternalState & RIGISING_STATE) == RIGISING_STATE; } }//�Ƿ�������
    public bool IsFalling { get { return (mInternalState & FALLING_STATE) == FALLING_STATE; } }//�Ƿ����½�
    public bool IsOnGround { get { return (mInternalState & ON_GROUND_STATE) == ON_GROUND_STATE; } }//�Ƿ��ڵ���
    public RaycastHit GroundRaycastHit { get { return mGroundRaycastHit; } }
    public event Action OnGroundRaycastHited;//��������Ļص�


    public void UpdateGroundDetection()//���µ�����
    {
        const float GROUND_DETECTE_LEN = 0.5f;//�����߶μ�ⳤ��
        var downAxis = Physics.gravity.normalized;//��ֱ���·���
        mInternalState = mInternalState & (~ON_GROUND_STATE);//���õ���״̬
        for (int i = 0; i < footPoints.Length; i++)//footPoints�ǽŲ�����
        {
            var raycastHit = default(RaycastHit);
            Debug.DrawRay(footPoints[i].position, downAxis,Color.red);
            if (Physics.Raycast(new Ray(footPoints[i].position, downAxis), out raycastHit, GROUND_DETECTE_LEN, groundLayerMask))
            {
                mInternalState |= ON_GROUND_STATE;//���õ���״̬
                mGroundRaycastHit = raycastHit;//����hit��Ϣ
                if (OnGroundRaycastHited != null)//��������ص�
                    OnGroundRaycastHited();
                break;
            }
        }
    }

    public void UpdateMovingStateDetect()
    {
        const float MOVING_EPS = 0.0001f;
        var upAxis = Physics.gravity.normalized;//��ֱ���Ϸ���
        mInternalState = mInternalState & (~MOVING_STATE);
        mInternalState = mInternalState & (~RIGISING_STATE);
        mInternalState = mInternalState & (~FALLING_STATE);//״̬����
        var velocity = transform.position - mLastPosition.GetValueOrDefault(transform.position);
        if (velocity.magnitude > MOVING_EPS)//�ƶ����ʴ�����˵�����ƶ�
        {
            mInternalState |= MOVING_STATE;
            var pitchValue = Vector3.Dot(velocity, upAxis);//��ֱ����ͶӰ
            if (pitchValue > 0) mInternalState |= RIGISING_STATE;//����0������״̬
            else if (pitchValue < 0) mInternalState |= FALLING_STATE;//С��0���½�״̬
        }
        mLastPosition = transform.position;//�����ϴ�����
    }

    void Update()
    {
        UpdateGroundDetection();
        UpdateMovingStateDetect();
        Debug.Log(IsOnGround);
    }
}
