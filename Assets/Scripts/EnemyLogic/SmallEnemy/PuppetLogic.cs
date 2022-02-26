using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class PuppetLogic : MonoBehaviour
{
    #region Value
    Rigidbody2D m_Rigidbody;
    SpriteRenderer m_SpriteRenderer;
    Animator EnemyAnim;
    public int EnemyCurrentHealth;
    public int MaxHP = 100;
    public int attackDamage=10;
    public EnemyState enemyState;
    public bool isPuppet;
    public EnemyAttack enemyAttack;
    private EnemyState m_originalState;
    private GameObject m_targetPlayer;
    Vector3 originScale;
    // public Text EnemyHealthText;
    // private int TimesOfPerfectParry = 0;
    public Collider2D m_collider;
   // NormalAttactBehaviour normalAttackBehaviour;


    [Header("Movement")]
    public Transform eye;
    //public float speed;
    public float chaseSpeed;
    public float rayLength;
   // public float patrolRadius;
    public float canChaseTargetRadius;
    public float attackRadius;
    private float m_direction;
    public float minimumDistanceWithPlayer;

    //float m_attackTimer;

    // Vector2 m_patrolStart;
    //Vector2 m_patrolEnd;

    //float m_patrolDuration;

    [HideInInspector]
    public Vector2 m_moveVelocity;

    [Header("Attack")]
    public bool canChangeState;
    public bool isAttacking;
    public bool isPlayAnim;
    public float dashSpeed;
    public float CloseAndFar;
    public bool dash;
    public bool continue1;
    public bool normal;
    public int wanderSec_low;
    public int wanderSec_high;

    [HideInInspector]
    public int hurtFrame;//the frame when player been hurt
    [HideInInspector]
    public float hurtForce;//the force when player been hurt

    [Header("The Attack Distance")]
    [Tooltip("When player and enemy is close enough within the distance, the attack anims will be played")]
    public float dashDistance;//the distance between player and enemy for dash distance
    public float normalDistance;//the distance between player and enemy for normal distance
    public float continue1Distance;//the distance between player and enemy for continue1 distance

    [Header("The hurt frame for each attack")]
    //the hurtFrame for each attack
    [Tooltip("Player will hurt for the certain frame after each kinds of attack")]
    public int dashFrame;
    public int normalFrame;
    public int continue1Frame;
    [Header("The hurt force for each attack")]
    //the hurtForce for each attack
    [Tooltip("Player will be hit back by this certain force")]
    public float dashForce;
    public float normalForce;
    public float continue1Force;

  //  [Header("UI")]
   // public Slider enemyhealthbar;
    [Header("Search")]
    public float searchSpeed;
    public GameObject[] searchPoint;
    int index;
    bool canChase;

    #endregion

    SpriteRenderer[] m_spriteRenderer;
    List<Material> m_selfMaterialShader = new List<Material>();
    const float M_INITIAFADEVALUE = 1.0f;
    const float M_ENDFADEVALUE = 0.0f;
    const float M_FADETIME = 1f;
    float m_fadeValue;
    private void Start()
    {
        m_spriteRenderer = GetComponentsInChildren<SpriteRenderer>();
        foreach (var spriteRender in m_spriteRenderer)
        {
            m_selfMaterialShader.Add(spriteRender.material);
        }
        m_Rigidbody = GetComponentInParent<Rigidbody2D>();
        EnemyAnim = GetComponent<Animator>();
        m_targetPlayer = GameObject.FindGameObjectWithTag("Player");
        InitializeState();
        index = 0;
        //UI
        EnemyCurrentHealth = MaxHP;
        originScale = transform.GetComponentInParent<Rigidbody2D>().transform.localScale;
        //EnemyHealthText.GetComponent<Text>();
        //EnemyHealthText.text = "EnemyHealth:" + EnemyCurrentHealth;

    }

    private void Update()
    {
        //if (enemyhealthbar != null)
        //    enemyhealthbar.value = EnemyCurrentHealth * 1.0f / MaxHP;
        if (EnemyCurrentHealth <= 0)
        {
            StartCoroutine(GradualChange(M_INITIAFADEVALUE, M_ENDFADEVALUE, M_FADETIME));
            enemyState = EnemyState.Idle;
            Invoke("EnemyDies", M_FADETIME);
            //EnemyDies();
        }
        //if (isSpike)
        //{
        //    SpikeAttack();
        //}
        AvoidCollidePlayer();
        if (enemyState == EnemyState.BeExecuted)
            m_moveVelocity.x = 0;
    }

    private void FixedUpdate()
    {
        ChangeState();
        ChangeMoveAnimation();

        m_moveVelocity.y = m_Rigidbody.velocity.y;
        m_Rigidbody.velocity = m_moveVelocity;
        //Debug.Log(m_Rigidbody.velocity);
        //m_attackTimer += Time.deltaTime;

    }

    #region function

    private void ChangeState()
    {
        switch (enemyState)
        {
            case EnemyState.Idle:
               // EnemyAnim.SetBool("IsAttacking", false);
                Search();
                break;
            case EnemyState.Attack:
             //   EnemyAnim.SetBool("IsAttacking", true);
                Attack();
                break;
            case EnemyState.Chase:
              //  EnemyAnim.SetBool("IsAttacking", false);
                Chase();
                break;
            default:
                break;
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
            case EnemyAttack.Continue1:
                Continue1Attack();
                // Debug.Log("Continue1");
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

    #region Move
    void Search()
    {
        if (m_targetPlayer)
        {
            EnemyFacing();
            float distance = Vector2.Distance(m_targetPlayer.transform.position, m_Rigidbody.transform.position);
            if (distance <= canChaseTargetRadius)
            {
                enemyState = EnemyState.Chase;
            }
            else if(searchPoint.Length>0)
            {
                float disToSearchPoint = Vector2.Distance(searchPoint[index].transform.position, m_Rigidbody.transform.position);
               // Debug.Log(disToSearchPoint);
                if(disToSearchPoint<4f)
                {
                    index = (index + 1) % searchPoint.Length;
                }
                m_moveVelocity.x = m_direction * (searchPoint[index].transform.position - transform.parent.position).normalized.x * searchSpeed;
            }
        }
    }
    void Chase()
    {
        if (m_targetPlayer)
        {
            EnemyFacing();
            float distance = Vector2.Distance(m_targetPlayer.transform.position, m_Rigidbody.transform.position);
            //Vector2 EnemyPos = transform.position;
            //Vector2 PlayerPos = m_targetPlayer.transform.position;
            //Vector2 Position = Vector2.MoveTowards(PlayerPos, EnemyPos, distance);
            // m_ChaseVelocity.x = Position.x * speed;
            if (distance <= attackRadius)
            {
                enemyState = EnemyState.Attack;
            }
            else if (distance >= canChaseTargetRadius)
            {
                BackToOriginalState();
            }
            else if(canChase)
            {
                StartChasingPlayer();
            }
        }
    }
    void Patrol()
    {
        m_moveVelocity.x = 0;
    }
    private void BackToOriginalState()
    {
        enemyState = m_originalState;
    }
    private void StartChasingPlayer()
    {        
        if (!isPlayAnim)
        {
            m_moveVelocity.x = m_direction * (m_targetPlayer.transform.position - transform.parent.position).normalized.x * chaseSpeed;
        }
        else
        {
            m_moveVelocity.x = 0;
        }
    }
    void ChangeMoveAnimation()
    {
        //EnemyAnim.SetFloat("HorizontalVelocity", Mathf.Abs(m_Rigidbody.velocity.x));
    }

    private void EnemyFacing()
    {
        float distanceSub;
        if (enemyState==EnemyState.Idle)
        {
            distanceSub = searchPoint[index].transform.position.x - transform.position.x;
        }
        else
        {
            distanceSub = m_targetPlayer.transform.position.x - transform.position.x;
        }
        if (distanceSub < 0)
        {
            transform.GetComponentInParent<Rigidbody2D>().transform.localScale = originScale;
        }
        if (distanceSub > 0)
        {
            transform.GetComponentInParent<Rigidbody2D>().transform.localScale = new Vector3( -originScale.x,originScale.y,originScale.z);
        }
    }
    #endregion

    #region Attack

    public void Attack()
    {
        m_moveVelocity.x = 0;
        //if (enemyState != EnemyState.BeExecuted || !isAttacking)
        EnemyFacing();
        //m_attackTimer = 0f;
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
    void SpecificAttackDetection()
    {
        float distance = Vector2.Distance(m_targetPlayer.transform.position, m_Rigidbody.transform.position);
            if (distance < CloseAndFar)
            {
                if (AttackChanceRate(50)&&normal)
                    enemyAttack = EnemyAttack.NormalAttack;
                else if(continue1)
                    enemyAttack = EnemyAttack.Continue1;
                else
                enemyAttack = EnemyAttack.None;
        }
            else  if(dash)
            {
            enemyAttack = EnemyAttack.DashAttack;
        }
            else
            enemyAttack = EnemyAttack.None;
    }
    public static bool AttackChanceRate(int percent)
    {
        return Random.Range(0, 100) <= percent;
    }
    #region attack func
    void DashAttack()
    {
        hurtFrame = dashFrame;
        hurtForce = dashForce;
        float distance = Vector2.Distance(m_targetPlayer.transform.position, transform.parent.position);
        //Dash and stop
        if (distance > dashDistance && !isAttacking&&canChase)
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

    void NormalAttack()
    {
        hurtFrame = normalFrame;
        hurtForce = normalForce;
        float distance = Vector2.Distance(m_targetPlayer.transform.position, transform.parent.position);
        //Dash and stop
        if (distance > normalDistance && !isAttacking&&canChase)
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
    void Continue1Attack()
    {
        hurtFrame = continue1Frame;
        hurtForce = continue1Force;
        float distance = Vector2.Distance(m_targetPlayer.transform.position, transform.parent.position);
        //Dash and stop
        if (distance > continue1Distance && !isAttacking&&canChase)
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
    #endregion

    #endregion

    private void OnDrawGizmos()
    {
        if (transform.parent != null)
        {
            Gizmos.color = Color.red;
            Gizmos.DrawWireSphere(transform.parent.position, attackRadius);
            Gizmos.color = Color.yellow;
            Gizmos.DrawWireSphere(transform.parent.position, normalDistance);
            Gizmos.color = Color.green;
            Gizmos.DrawWireSphere(transform.parent.position, canChaseTargetRadius);
            Gizmos.color = Color.black;
            //Gizmos.DrawWireSphere(transform.parent.position, Radius1);
            //Gizmos.color = Color.cyan;
            //Gizmos.DrawWireSphere(transform.parent.position, Radius2);
            //Gizmos.color = Color.gray;
            //Gizmos.DrawWireSphere(transform.parent.position, Radius3);
        }
    }
    private void InitializeState()
    {
        enemyState = EnemyState.Idle;
        m_originalState = enemyState;
        //m_patrolStart = transform.position + Vector3.left * patrolRadius;
        //m_patrolEnd = transform.position + Vector3.right * patrolRadius;
        //m_patrolDuration = (m_patrolEnd - m_patrolStart).magnitude / speed;
        canChangeState = true;
        m_direction = 1;
        //if (enemyhealthbar != null)
        //    enemyhealthbar.value = 1;
    }
    private void EnemyDies()
    {
        Destroy(this.transform.parent.gameObject);
    }

    void AvoidCollidePlayer()
    {
        GameObject collider;
        if (isFaceWall(eye.position, rayLength, out collider))
        {

            //if (enemyAttack != EnemyAttack.Wander)
            //{
            //    m_moveVelocity.x = 0f;
            //}
            if(enemyState==EnemyState.Chase|| enemyState == EnemyState.Attack)
            {
               // Debug.Log("enter");
                canChase = false;
                m_moveVelocity.x = 0f;
            }
            //if (enemyAttack != EnemyAttack.Wander)
            //{
            //    Debug.Log("enter1");
            //    m_moveVelocity.x = 0f;
            //}
            //else if (Mathf.Abs(this.transform.position.x - m_targetPlayer.transform.position.x) >= minimumDistanceWithPlayer)
            //{
            //    Debug.Log("enter2");
            //    m_direction = (this.transform.position.x - collider.transform.position.x) / Mathf.Abs(this.transform.position.x - collider.transform.position.x);
            //}
            //else
            //{
            //    Debug.Log("enter3");
            //    m_direction = 0;
            //}
        }
        else
        {
            canChase =true;
        }
        //else if (Mathf.Abs(this.transform.position.x - m_targetPlayer.transform.position.x) < minimumDistanceWithPlayer)
        //{
        //    //Debug.Log(m_direction);
        //    //m_direction = 0;
        //    //m_direction = -(this.transform.position.x - m_targetPlayer.transform.position.x) / Mathf.Abs(this.transform.position.x - m_targetPlayer.transform.position.x);
        //    //Debug.Log(m_direction);
        //}
    }
    bool isFaceWall(Vector2 eye, float rayLength, out GameObject collider)
    {
        Vector2 rayStart = eye;
        Debug.DrawRay(rayStart, new Vector2(-transform.GetComponentInParent<Rigidbody2D>().transform.localScale.x / originScale.x, 0) * rayLength);
        RaycastHit2D ray1 = Physics2D.Raycast(rayStart, new Vector2(-transform.GetComponentInParent<Rigidbody2D>().transform.localScale.x/originScale.x,0), rayLength, 1 << 8);
        if ((ray1.collider != null && ray1.collider.tag == "Wall"))
        {
            collider = ray1.collider.gameObject;
            return true;
        }
        else
        {
            collider = null;
            return false;
        }

    }
    #endregion

    IEnumerator GradualChange(float initialValue, float endValue, float seconds)
    {
        Vector2 initialPos = new Vector2(0, initialValue);
        Vector2 endPos = new Vector2(seconds, endValue);
        float t = 0f;
        while (t <= 1f)
        {
            t += Time.deltaTime / seconds;
            m_fadeValue = Vector2.Lerp(initialPos, endPos, t).y;
            foreach (var shaderMat in m_selfMaterialShader)
            {
                shaderMat.SetFloat("_Fade", m_fadeValue);
            }
            yield return null;
        }
    }
    #region ChangeParameters
    void ResetTriggersForAttack()
    {
        isPlayAnim = false;
        EnemyAnim.ResetTrigger("CanDashAttack");
        EnemyAnim.ResetTrigger("CanNormalAttack");
        EnemyAnim.ResetTrigger("CanContinue1");
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
    void OpenTheWeaponCollider()
    {
        m_collider.enabled = true;
    }
    public void CloseTheWeaponCollider()
    {
        m_collider.enabled = false;
    }
    IEnumerator IdleBetweenAttack()
    {
        enemyAttack = EnemyAttack.Wander;
        //float distance = Vector2.Distance(m_targetPlayer.transform.position, transform.parent.position);
        //if (distance > Radius2)
        //    m_direction = (m_targetPlayer.transform.position.x - transform.parent.position.x) / Mathf.Abs(m_targetPlayer.transform.position.x - transform.parent.position.x);
        //else if (distance < Radius1)
        //    m_direction = -(m_targetPlayer.transform.position.x - transform.parent.position.x) / Mathf.Abs(m_targetPlayer.transform.position.x - transform.parent.position.x);
        //else
        //    m_direction = Random.Range(0, 2) == 0 ? -1 : 1;//the patrol direction of enemy
        yield return new WaitForSeconds(Random.Range(wanderSec_low, wanderSec_high) * Time.deltaTime);
        canChangeState = true;
    }
    void OpenTheTriggerForStateChange()
    {
        //Debug.Log("ENTER");
        StartCoroutine(IdleBetweenAttack());
    }
    #endregion
}
