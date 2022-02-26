using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Cinemachine;

public class ExecuteCamera : MonoBehaviour
{
    CinemachineVirtualCamera vcam;
    public float m_fieldOfView;
    private float origin_fieldofview;
    Player m_player;

    private void Awake()
    {
        vcam = GetComponent<CinemachineVirtualCamera>();
        origin_fieldofview = vcam.m_Lens.FieldOfView;
        m_player = GameObject.FindObjectOfType<Player>();
    }
    void Update()
    {

        if (m_player.currentState==Player.PlayerState.Execute)
        {
          //  vcam.m_Lens.FieldOfView = m_fieldOfView;
          if(vcam.m_Lens.FieldOfView>m_fieldOfView)
            {
                vcam.m_Lens.FieldOfView -= 2;
            }
        }
        else
        {
            if (vcam.m_Lens.FieldOfView < origin_fieldofview)
            {
                vcam.m_Lens.FieldOfView += 2;
            }
        }
    }
}
