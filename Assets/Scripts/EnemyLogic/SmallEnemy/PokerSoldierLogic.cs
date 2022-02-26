using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class PokerSoldierLogic : MonoBehaviour
{
    #region Value
    Rigidbody2D m_Rigidbody;
    SpriteRenderer m_SpriteRenderer;
    Animator EnemyAnim;
    public int EnemyCurrentHealth;
    public int MaxHP = 100;
    // public Text EnemyHealthText;
    private int TimesOfPerfectParry = 0;
    public Collider2D m_collider;
    NormalAttactBehaviour normalAttackBehaviour;

    [Header("Movement")]
    public Transform eye;
    public float speed;
    public float chaseSpeed;
    public float rayLength;


    public float patrolRadius;
    public float canChaseTargetRadius;
    public float attackRadius;



    public EnemyState enemyState;
    public EnemyAttack enemyAttack;

    private EnemyState m_originalState;
    private GameObject m_targetPlayer;
    private float m_direction;

    float m_attackTimer;

    Vector2 m_patrolStart;
    Vector2 m_patrolEnd;

    float m_patrolDuration;

    [HideInInspector]
    public Vector2 m_moveVelocity;
    public bool canChangeState;

    [Header("UI")]
    public Slider enemyhealthbar;

    #endregion

    private void Start()
    {
        m_Rigidbody = GetComponentInParent<Rigidbody2D>();
        EnemyAnim = GetComponent<Animator>();
        m_targetPlayer = GameObject.FindGameObjectWithTag("Player");
        InitializeState();
        //UI
        EnemyCurrentHealth = MaxHP;
        //EnemyHealthText.GetComponent<Text>();
        //EnemyHealthText.text = "EnemyHealth:" + EnemyCurrentHealth;

    }

    private void Update()
    {
        if (enemyhealthbar != null)
            enemyhealthbar.value = EnemyCurrentHealth * 1.0f / MaxHP;
        if (EnemyCurrentHealth <= 0)
        {
            EnemyDies();
        }
        //if (isSpike)
        //{
        //    SpikeAttack();
        //}
        if (enemyState == EnemyState.BeExecuted)
            m_moveVelocity.x = 0;
    }

    private void FixedUpdate()
    {
        ChangeState();
        ChangeMoveAnimation();
        m_moveVelocity.y = m_Rigidbody.velocity.y;
        m_Rigidbody.velocity = m_moveVelocity;
        m_attackTimer += Time.deltaTime;

    }

    #region function

    private void ChangeState()
    {
        switch (enemyState)
        {
            case EnemyState.Idle:
                EnemyAnim.SetBool("IsAttacking", false);
                Search();
                break;
            case EnemyState.Attack:
                EnemyAnim.SetBool("IsAttacking", true);
                Attack();
                break;
            case EnemyState.Chase:
                EnemyAnim.SetBool("IsAttacking", false);
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
            //case EnemyAttack.NormalAttack:
            //    NormalAttack();
            //    // Debug.Log("NormalAttack");
            //    break;
            //case EnemyAttack.DashAttack:
            //    DashAttack();
            //    //  Debug.Log("DashAttack");
            //    break;
            //case EnemyAttack.Continue1:
            //    Continue1Attack();
            //    // Debug.Log("Continue1");
            //    break;
            //case EnemyAttack.Continue2:
            //    Continue2Attack();
            //    // Debug.Log("Continue2");
            //    break;
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
            float distance = Vector2.Distance(m_targetPlayer.transform.position, m_Rigidbody.transform.position);
            if (distance <= canChaseTargetRadius)
            {
                enemyState = EnemyState.Chase;
            }
            m_moveVelocity.x = 0f;
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
            else
            {
                StartChasingPlayer();
            }
        }
    }
    void Patrol()
    {
        m_moveVelocity.x = m_direction * speed;
    }
    private void BackToOriginalState()
    {
        enemyState = m_originalState;
    }
    private void StartChasingPlayer()
    {
        m_moveVelocity.x = (m_targetPlayer.transform.position - transform.parent.position).normalized.x * chaseSpeed;
    }
    void ChangeMoveAnimation()
    {
        EnemyAnim.SetFloat("HorizontalVelocity", Mathf.Abs(m_Rigidbody.velocity.x));
    }

    private void EnemyFacing()
    {
        float distanceSub = m_targetPlayer.transform.position.x - transform.position.x;
        if (distanceSub < 0)
        {
            transform.GetComponentInParent<Rigidbody2D>().transform.localScale = new Vector3(1f, 1f, 1f);
        }
        if (distanceSub > 0)
        {
            transform.GetComponentInParent<Rigidbody2D>().transform.localScale = new Vector3(-1f, 1f, 1f);
        }
    }
    #endregion

    #region Attack

    public void Attack()
    {
        //if (enemyState != EnemyState.BeExecuted || !isAttacking)
        EnemyFacing();
        m_attackTimer = 0f;
        if (canChangeState)
        {
            //SpecificAttackDetection();
            canChangeState = false;
        }

        //ChangeAttackState();

        float distance = Vector2.Distance(m_targetPlayer.transform.position, transform.position);
        if (distance >= attackRadius)
        {
            enemyState = EnemyState.Chase;
        }
    }
    #endregion

    private void OnDrawGizmos()
    {
        if (transform.parent != null)
        {
            Gizmos.color = Color.red;
            Gizmos.DrawWireSphere(transform.parent.position, attackRadius);
            // Gizmos.color = Color.yellow;
            // Gizmos.DrawWireSphere(transform.position, searchRadius);
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
        m_patrolStart = transform.position + Vector3.left * patrolRadius;
        m_patrolEnd = transform.position + Vector3.right * patrolRadius;
        m_patrolDuration = (m_patrolEnd - m_patrolStart).magnitude / speed;
        canChangeState = true;
        m_direction = 1;
        if(enemyhealthbar!=null)
            enemyhealthbar.value = 1;
    }
    private void EnemyDies()
    {
        Destroy(this.transform.parent.gameObject);
    }

    //void AvoidCollidePlayer()
    //{
    //    GameObject collider;

    //    if (isFaceWall(eye.position, rayLength, out collider))
    //    {

    //        if (enemyAttack != EnemyAttack.Wander)
    //        {
    //            Debug.Log("enter1");
    //            m_moveVelocity.x = 0f;
    //        }
    //        else if (Mathf.Abs(this.transform.position.x - m_targetPlayer.transform.position.x) >= downDistance)
    //        {
    //            Debug.Log("enter2");
    //            m_direction = (this.transform.position.x - collider.transform.position.x) / Mathf.Abs(this.transform.position.x - collider.transform.position.x);
    //        }
    //        else
    //        {
    //            Debug.Log("enter3");
    //            m_direction = 0;
    //        }
    //    }
    //    else if (Mathf.Abs(this.transform.position.x - m_targetPlayer.transform.position.x) < downDistance)
    //    {
    //        m_direction = (this.transform.position.x - m_targetPlayer.transform.position.x) / Mathf.Abs(this.transform.position.x - m_targetPlayer.transform.position.x);
    //    }
    //}
    bool isFaceWall(Vector2 eye, float rayLength, out GameObject collider)
    {
        Vector2 rayStart = eye;
        Debug.DrawRay(rayStart, Vector2.right * rayLength);//将射线显示出来
        Debug.DrawRay(rayStart, -Vector2.right * rayLength);//将射线显示出来
        RaycastHit2D ray1 = Physics2D.Raycast(rayStart, Vector2.right, rayLength, 1 << 8);
        RaycastHit2D ray2 = Physics2D.Raycast(rayStart, -Vector2.right, rayLength, 1 << 8);
        if ((ray1.collider != null && ray1.collider.tag == "Wall") || (ray2.collider != null && ray2.collider.tag == "Wall"))
        {
            collider = ray1.collider == null ? ray2.collider.gameObject : ray1.collider.gameObject;
            return true;
        }
        else
        {
            collider = null;
            return false;
        }

    }
    #endregion
}
