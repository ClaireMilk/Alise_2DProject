using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum EnemyAttack
{
    None,
    Wander,
    DownAttack,
    DashAttack,
    NormalAttack,
    Continue1,
    Continue2,
    SmallDown,
    SpikeMagic
}

public partial class Enemy : MonoBehaviour
{

    #region Variable
    [HideInInspector]
    public bool canChangeState;
    [Header("Attack")]
    public EnemyAttack enemyAttack;
    public bool isState2;
    [Range(10, 20)]
    public int wanderFrame;
    [Tooltip("The damage for each attack")]
    public int attackDamage;//the damage to player
    [Header("The Range For Triggering Different Attacks")]
    public float Radius1;//range for down attack
    public float Radius2;//range for normal attack
    public float Radius3;//range for dash attack
    [Tooltip("This is the speed for dash")]
    public float dashSpeed;
    [Header("The Attack Distance")]
    [Tooltip("When player and enemy is close enough within the distance, the attack anims will be played")]
    public float dashDistance;//the distance between player and enemy for dash distance
    public float downDistance;//the distance between player and enemy for down distance
    public float normalDistance;//the distance between player and enemy for normal distance
    public float continue1Distance;//the distance between player and enemy for continue1 distance
    public float continue2Distance;//the distance between player and enemy for continue2l distance
    public float smallDownDistance;//the distance between player and enemy for continue2l distance
    public float SpikeDistance;//the distance between player and enemy for continue2l distance
    [Header("The hurt frame for each attack")]
    //the hurtFrame for each attack
    [Tooltip("Player will hurt for the certain frame after each kinds of attack")]
    public int dashFrame;
    public int downFrame;
    public int normalFrame;
    public int continue1Frame;
    public int continue2Frame;
    public int smallDownFrame;
    public int SpikeFrame;
    [Header("The hurt force for each attack")]
    //the hurtForce for each attack
    [Tooltip("Player will be hit back by this certain force")]
    public float dashForce;
    public float downForce;
    public float normalForce;
    public float continue1Force;
    public float continue2Force;
    public float smallDownForce;
    public float SpikeForce;
    [HideInInspector]
    public int hurtFrame;//the frame when player been hurt
    [HideInInspector]
    public float hurtForce;//the force when player been hurt



    [Header("AttackChance")]
    public int dashAttackRate;
    public int Attact2Rate;//the rate for radius2 attack
    public int downAttactRate;
    public int normalAttactRate;
    public int continue1Rate;
    public int State2AttackRate;
    //public bool oneTimeDashAttactDetection = true;
    //public bool oneTimeNormalAttactDetection = true;

    [Header("Spike")]
    public int intervalDis;
    float intervalTime = 0.2f;
    float timer;
    public GameObject m_spike;
    GameObject spike1;
    GameObject spike2;
    bool isSpike;

    bool isAttacking;
    public bool isPlayAnim;
    [HideInInspector]
    public bool isDisappear;
    #endregion

    List<GameObject> m_leftSpikes = new List<GameObject>();
    List<GameObject> m_rightSpikes = new List<GameObject>();
    int m_spikeNum = 5;
    int m_index = 0;
    public void Attack()
    {
        //Debug.Log(isAttacking);
        if (enemyState != EnemyState.BeExecuted || !isAttacking)
            EnemyFacing();
        //Apply Damage to Player.
        m_attackTimer = 0f;
        if (canChangeState)
        {
            SpecificAttackDetection();
            canChangeState = false;
        }

        ChangeAttackState();

        float distance = Vector2.Distance(m_targetPlayer.transform.position, transform.position);
        if (distance >= attackRadius)
        {
            enemyState = EnemyState.Chase;
        }
    }

    void ChangeAttackState()
    {
        //Debug.Log(isPlayAnim);
        switch (enemyAttack)
        {
            case EnemyAttack.NormalAttack:
                NormalAttack();
                // Debug.Log("NormalAttack");
                break;
            case EnemyAttack.DashAttack:
                DashAttack();
                //  Debug.Log("DashAttack");
                break;
            case EnemyAttack.DownAttack:
                DownAttack();
                //  Debug.Log("DownAttack");
                break;
            case EnemyAttack.Continue1:
                Continue1Attack();
                // Debug.Log("Continue1");
                break;
            case EnemyAttack.Continue2:
                Continue2Attack();
                // Debug.Log("Continue2");
                break;
            case EnemyAttack.SmallDown:
                SmallDown();
                break;
            case EnemyAttack.SpikeMagic:
                SpikeMagic();
                break;
            case EnemyAttack.Wander:
                if (enemyState != EnemyState.BeExecuted)
                    Patrol();
                break;

            default:
                canChangeState = true;
                break;
        }
    }
    /// <summary>
    /// decide which attack the enemy will trigger
    /// </summary>
    void SpecificAttackDetection()
    {
        float distance = Vector2.Distance(m_targetPlayer.transform.position, m_Rigidbody.transform.position);
        if (isState2)
        {
            if (AttackChanceRate(State2AttackRate))
            {
                int percentage = Random.Range(0, 100);
                if (percentage < 50)
                {
                    enemyAttack = EnemyAttack.SmallDown;
                }
                else
                {
                    enemyAttack = EnemyAttack.SpikeMagic;
                }
            }
        }
        if (enemyAttack != EnemyAttack.SmallDown && enemyAttack != EnemyAttack.SpikeMagic)
        {
            if (distance < Radius1)
            {
                if (AttackChanceRate(downAttactRate))
                    enemyAttack = EnemyAttack.DownAttack;
                else if (AttackChanceRate(State2AttackRate))
                    enemyAttack = EnemyAttack.SmallDown;
                else
                    enemyAttack = EnemyAttack.None;
            }
            else if (distance < Radius2)
            {
                if (AttackChanceRate(Attact2Rate))
                {
                    int percentage = Random.Range(0, 100);
                    if (percentage < normalAttactRate)
                    {
                        enemyAttack = EnemyAttack.NormalAttack;
                    }
                    else if (percentage < continue1Rate)
                    {
                        enemyAttack = EnemyAttack.Continue1;
                    }
                    else
                    {
                        enemyAttack = EnemyAttack.Continue2;
                    }
                }
                else
                    enemyAttack = EnemyAttack.None;
            }
            else if (distance < Radius3)
            {
                if (AttackChanceRate(dashAttackRate))
                    enemyAttack = EnemyAttack.DashAttack;
                else
                    enemyAttack = EnemyAttack.None;
            }
            else
            {
                enemyAttack = EnemyAttack.None;
            }
        }

    }


    #region attack func
    void DashAttack()
    {
        hurtFrame = dashFrame;
        hurtForce = dashForce;
        float distance = Vector2.Distance(m_targetPlayer.transform.position, transform.parent.position);
        //Dash and stop
        if (distance > dashDistance && !isAttacking)
        {
            m_moveVelocity.x = (m_targetPlayer.transform.position - transform.parent.position).normalized.x * dashSpeed;
        }
        else
            m_moveVelocity.x = 0;
        if (!isPlayAnim)
        {
            isPlayAnim = true;
            EnemyAnim.SetTrigger("CanDashAttack");

        }

    }
    void SmallDown()
    {
        hurtForce = smallDownForce;
        hurtFrame = smallDownFrame;
        if (!isAttacking)
            m_moveVelocity.x = (m_targetPlayer.transform.position - transform.parent.position).normalized.x * chaseSpeed;
        else
            m_moveVelocity.x = 0;
        if (!isPlayAnim)
        {
            isDisappear = true;
            isPlayAnim = true;
            EnemyAnim.SetTrigger("CanSmallDown");
            Invoke("SmallDown_2", 3.0f);
        }
    }
    void SmallDown_2()
    {
        isDisappear = false;
        if (m_targetPlayer.transform.position.x < 0)
            this.transform.parent.position = new Vector3(smallDownDistance + m_targetPlayer.transform.position.x, this.transform.parent.position.y, this.transform.parent.position.z);
        else
            this.transform.parent.position = new Vector3(m_targetPlayer.transform.position.x - smallDownDistance, this.transform.parent.position.y, this.transform.parent.position.z);
        isPlayAnim = true;
        EnemyAnim.SetTrigger("CanSmallDown02");
    }
    void NormalAttack()
    {
        hurtFrame = normalFrame;
        hurtForce = normalForce;
        float distance = Vector2.Distance(m_targetPlayer.transform.position, transform.parent.position);
        //Dash and stop
        if (distance > normalDistance && !isAttacking)
        {
            m_moveVelocity.x = (m_targetPlayer.transform.position - transform.parent.position).normalized.x * chaseSpeed;
        }
        else
        {
            m_moveVelocity.x = 0;
            if (!isPlayAnim)
            {
                isPlayAnim = true;
                EnemyAnim.SetTrigger("CanNormalAttack");
            }
        }
    }
    void SpikeMagic()
    {
        hurtFrame = SpikeFrame;
        hurtForce = SpikeForce;
        float distance = Vector2.Distance(m_targetPlayer.transform.position, transform.parent.position);
        if (!isAttacking)
        {
            m_moveVelocity.x = -(m_targetPlayer.transform.position - transform.parent.position).normalized.x * chaseSpeed;
        }
        else
        {
            m_moveVelocity.x = 0;
        }
        if (!isPlayAnim)
        {
            isPlayAnim = true;
            EnemyAnim.SetTrigger("CanSpikeMagic");
        }
    }
    void DownAttack()
    {
        hurtFrame = downFrame;
        hurtForce = downForce;
        float distance = Vector2.Distance(m_targetPlayer.transform.position, transform.parent.position);
        //Dash and stop
        if (distance > downDistance && !isAttacking)
        {
            //Debug.Log("buxing");
            m_moveVelocity.x = (m_targetPlayer.transform.position - transform.parent.position).normalized.x * chaseSpeed;
        }
        else
        {
            m_moveVelocity.x = 0;
            if (!isPlayAnim)
            {
                isPlayAnim = true;
                EnemyAnim.SetTrigger("CanDownAttack");
            }
        }

    }
    void Continue1Attack()
    {
        hurtFrame = continue1Frame;
        hurtForce = continue1Force;
        float distance = Vector2.Distance(m_targetPlayer.transform.position, transform.parent.position);
        //Dash and stop
        if (distance > continue1Distance && !isAttacking)
        {
            m_moveVelocity.x = (m_targetPlayer.transform.position - transform.parent.position).normalized.x * chaseSpeed;
        }
        else
        {
            m_moveVelocity.x = 0;
            //Debug.Log("12");
            if (!isPlayAnim)
            {
                isPlayAnim = true;
                EnemyAnim.SetTrigger("CanContinue1");
            }
        }

    }
    void Continue2Attack()
    {
        hurtFrame = continue2Frame;
        hurtForce = continue2Force;
        float distance = Vector2.Distance(m_targetPlayer.transform.position, transform.parent.position);
        //Dash and stop
        if (distance > continue2Distance && !isAttacking)
        {
            m_moveVelocity.x = (m_targetPlayer.transform.position - transform.parent.position).normalized.x * chaseSpeed;
        }
        else
        {
            m_moveVelocity.x = 0;
            if (!isPlayAnim)
            {
                isPlayAnim = true;
                EnemyAnim.SetTrigger("CanContinue2");
            }
        }

    }
    #endregion


    // Chance of attack
    public static bool AttackChanceRate(int percent)
    {
        return Random.Range(0, 100) <= percent;
    }
    /// <summary>
    /// Use to instantiateSpike
    /// </summary>
    void SpikeAttack()
    {

        if (timer > intervalTime)
        {
            timer = 0;
            StartInstantiateSpikes(m_index + 1);
         //   Debug.Log("StartInstantiate");
            if (m_index >= 1)
            {
                if (m_rightSpikes[m_index - 1])
                {
                    StartCoroutine(SpikeMovement(m_rightSpikes[m_index - 1]));
                    StartCoroutine(SpikeMovement(m_leftSpikes[m_index - 1]));
                }
              //  Debug.Log("Movement");
            }

            m_index++;
        }
        timer += Time.deltaTime;



        //if (timer < intervalTime)
        //{
        //    timer += Time.deltaTime;
        //}
        //else
        //{
        //    timer = 0;
        //    spike1.transform.position += new Vector3(intervalDis, 0, 0);
        //    spike2.transform.position -= new Vector3(intervalDis, 0, 0);
        //}

    }
    public IEnumerator SpikeMovement(GameObject spike)
    {
        Vector3 originalPos = spike.transform.position;
        Vector3 newPos = originalPos - new Vector3(0f, 5f);
        float t = 0f;
        while (t <= 1f)
        {
            t += Time.deltaTime;
            spike.transform.position = Vector3.Lerp(originalPos, newPos, t);
            yield return null;
        }
        if (spike != null)
        {
            Destroy(spike);
        }

    }
    void StartInstantiateSpikes(int i)
    {
        GameObject newLeftSpike = Instantiate(m_spike, new Vector3(transform.parent.position.x + 2f * i, transform.parent.position.y + 1f, transform.parent.position.z), transform.rotation);
        m_leftSpikes.Add(newLeftSpike);
        GameObject newRightSpike = Instantiate(m_spike, new Vector3(transform.parent.position.x - 2f * i, transform.parent.position.y + 1f, transform.parent.position.z), transform.rotation);
        m_rightSpikes.Add(newRightSpike);
    }

    IEnumerator IdleBetweenAttack()
    {
        enemyAttack = EnemyAttack.Wander;
        float distance = Vector2.Distance(m_targetPlayer.transform.position, transform.parent.position);
        if (distance > Radius2)
            m_direction = (m_targetPlayer.transform.position.x - transform.parent.position.x) / Mathf.Abs(m_targetPlayer.transform.position.x - transform.parent.position.x);
        else if (distance < Radius1)
            m_direction = -(m_targetPlayer.transform.position.x - transform.parent.position.x) / Mathf.Abs(m_targetPlayer.transform.position.x - transform.parent.position.x);
        else
            m_direction = Random.Range(0, 2) == 0 ? -1 : 1;//the patrol direction of enemy
        yield return new WaitForSeconds(Random.Range(40, 80) * Time.deltaTime);
        canChangeState = true;
    }
    //private void ResetDashAttactOneTimeDetection()
    //{
    //    oneTimeDashAttactDetection = true;
    //    canDashAttack = false;
    //    ChangeDashAttackAnimation();
    //}
    //private void ResetNormalAttactOneTimeDetection()
    //{
    //    oneTimeNormalAttactDetection = true;
    //    canNormalAttack = false;
    //    ChangeNormalAttackAnimation();
    //}
    #region AnimationEvent
    void OpenTheWeaponCollider()
    {
        m_collider.enabled = true;
    }
    public void CloseTheWeaponCollider()
    {
        m_collider.enabled = false;
    }
    void OpenTheTriggerForStateChange()
    {
        //Debug.Log("ENTER");
        StartCoroutine(IdleBetweenAttack());
    }
    void ResetTriggersForAttack()
    {
        isPlayAnim = false;
        EnemyAnim.ResetTrigger("CanDashAttack");
        EnemyAnim.ResetTrigger("CanNormalAttack");
        EnemyAnim.ResetTrigger("CanDownAttack");
        EnemyAnim.ResetTrigger("CanContinue1");
        EnemyAnim.ResetTrigger("CanContinue2");
        EnemyAnim.ResetTrigger("CanSmallDown");
        EnemyAnim.ResetTrigger("CanSmallDown02");
        EnemyAnim.ResetTrigger("CanSpikeMagic");
    }
    void SetIsAttackingTrue()
    {
        isAttacking = true;
    }
    void SetIsAttackingFalse()
    {
        isAttacking = false;
        isPlayAnim = false;
    }
    void CancelPreparation()
    {
        float distance = Vector2.Distance(m_targetPlayer.transform.position, transform.parent.position);
        if (distance < Radius1)
            enemyAttack = EnemyAttack.DownAttack;
    }
    void InstantiateSpikes()
    {
        isSpike = true;
        //spike1 = GameObject.Instantiate(m_spike, new Vector3(this.transform.parent.position.x + intervalDis, this.transform.parent.position.y + 2, this.transform.parent.position.z), transform.rotation);
        //spike2 = GameObject.Instantiate(m_spike, new Vector3(this.transform.parent.position.x - intervalDis, this.transform.parent.position.y + 2, this.transform.parent.position.z), transform.rotation);

    }
    void StopSpikes()
    {
        isSpike = false;
        foreach (var spike in m_leftSpikes)
        {
            if (spike != null)
            {
                Destroy(spike);
            }
        }
        foreach (var spike in m_rightSpikes)
        {
            if (spike != null)
            {
                Destroy(spike);
            }
        }
        m_leftSpikes.Clear();
        m_rightSpikes.Clear();
        m_index = 0;
       // Debug.Log("Return");
        StopAllCoroutines();

        //Destroy(spike1);
        //Destroy(spike2);
    }
    #endregion
}
