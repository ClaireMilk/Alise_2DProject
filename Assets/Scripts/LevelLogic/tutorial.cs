using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Cinemachine;

public class tutorial : MonoBehaviour
{
    public GameObject m_player;
    public GameObject m_enemy;
    public GameObject m_camera;
    public GameObject m_cameraPoint;
    bool isStop;
    bool isNonPerfect;
    bool isPerfect;
    bool isExecute;
    private float dis;
    [Header("Tips")]
    public GameObject step1;
    public GameObject step2;
    public GameObject step3;
    public GameObject step4;
    enum TutorialState
    {
        step0,
        step1,
        step2,
        step3,
        step4
    }
    TutorialState state = TutorialState.step0;
    private void Start()
    {
        step1.SetActive(false);
        step2.SetActive(false);
        step3.SetActive(false);
        step4.SetActive(false);
    }
    private void Update()
    {
        if (dis < 20 && state == TutorialState.step0)
        {
            MoveToward();
        }
        if (state == TutorialState.step4)
        {
            m_player.GetComponentInChildren<Player>().currentState = Player.PlayerState.Execute;
        }

        dis =Vector3.Distance(m_player.transform.position, m_enemy.transform.position);
        if(dis<20&& state == TutorialState.step0)
        {
            //stop and show step1
            m_camera.GetComponent<CinemachineVirtualCamera>().Follow = m_cameraPoint.transform;
            //step1.SetActive(true);
           // Invoke("StopAnimationSpeed", 0.5f);
            //set state to step1
            //state = TutorialState.step1;
        }
        //if(Input.GetButtonDown("OK"))
        //{
        //    m_player.GetComponentInChildren<Player>().RecoverAnimatorParameter();
        //    //if step1
        //    if (state == TutorialState.step1)
        //    {
        //        //resume
        //        Time.timeScale = 1;
        //        step1.SetActive(false);
        //        //enemy walks toward player and stop at the front and play the attack anim
        //    }

        //}
        //if step2
        if (state == TutorialState.step2&&Input.GetButtonDown("Parry")&&!isNonPerfect)
        {
            step2.SetActive(false);
            Time.timeScale = 1;
            //出发非完美格挡
            m_player.GetComponentInChildren<Player>().NonPerfectParry(this.transform.GetChild(0).gameObject);
            isNonPerfect = true;
        }
        if (state == TutorialState.step3 && Input.GetButtonDown("Parry") && !isPerfect)
        {
            step3.SetActive(false);
            m_player.GetComponentInChildren<Player>().execution_vfx.SetActive(true);
            Time.timeScale = 1;
            //出发完美格挡
            m_player.GetComponentInChildren<Player>().PerfectParry(this.transform.GetChild(0).gameObject);
            isPerfect = true;
        }
        if (state == TutorialState.step4 && Input.GetButtonDown("Attack") && !isExecute)
        {
            isExecute = true;
            step4.SetActive(false);
            m_player.GetComponentInChildren<Player>().execution_vfx.SetActive(false);
            Time.timeScale = 1;
            m_player.GetComponentInChildren<Player>().RecoverAnimatorParameter();
            m_player.GetComponentInChildren<Player>().canInput=false;
            m_camera.GetComponent<CinemachineVirtualCamera>().Follow = m_player.transform;
        }
            //show unperfect parry and 
            //if (m_enemy.GetComponent<PuppetLogic>().isAttacking)
            //  {
            //slow down and show parry tips
            //recover the speed until player parry
            //perfect parry
            //setstepto step2
            // }

            //Enemy move toward player
            void MoveToward()
        {
            //near and attack
            if(!isStop)
                m_enemy.transform.position=Vector3.MoveTowards(m_enemy.transform.position, m_player.transform.position, 0.1f*Time.timeScale);
            dis = Vector3.Distance(m_player.transform.position, m_enemy.transform.position);
            if(dis<10)
            {
                isStop = true;
                m_enemy.GetComponentInChildren<Animator>().SetTrigger("step2");
            }
        }

    }
    //attack stop point
    public void StopAnimation1()
    {
        //stop and show step2
        Time.timeScale = 0;
        step2.SetActive(true);
        state = TutorialState.step2;
    }
    public void StopAnimation2()
    {
        //stop and show step2
        Time.timeScale = 0;
        step3.SetActive(true);
        state = TutorialState.step3;
        Invoke("ExecutionTips", 1.0f);
    }
    public void ExecutionTips()
    {
        step4.SetActive(true);
        Time.timeScale = 0;
        state = TutorialState.step4;
        m_player.GetComponentInChildren<Player>().canExecute = true;
    }
    public void StopAnimationSpeed()
    {
        Time.timeScale = 0f;
    }
    public void RecoverAnimationSpeed()
    {
        Time.timeScale = 1f;
    }
}
