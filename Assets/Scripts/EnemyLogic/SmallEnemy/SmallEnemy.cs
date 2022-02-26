using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum SmallEnemyState
{
    Idle,
    Patrol,
    Attack,
    Chase,
}

public class SmallEnemy : MonoBehaviour
{
    Rigidbody2D m_Rigidbody;
    SpriteRenderer m_SpriteRenderer;
    public Collider2D m_collider;
    Animator enemyAnim;
    public GameObject weaponCollider;

    [Header("Movement")]
    public float speed;
    public float chaseSpeed;
    public float rayLength;
    public float xOffSet;

    public float patrolRadius;
    public float searchRadius;
    public float loseTargetRadius;
    public float attackRadius;



    public SmallEnemyState enemyState;

    private SmallEnemyState m_originalState;
    public GameObject m_targetPlayer;
    Vector2 m_patrolStart;
    Vector2 m_patrolEnd;
    float m_patrolDuration;
    Vector2 m_moveVelocity;
    private float m_direction;

    [HideInInspector]
    public int hurtFrame;//the frame when player been hurt
    [HideInInspector]
    public float hurtForce;//the force when player been hurt

    void Start()
    {
        enemyAnim = GetComponentInParent<Animator>();
        m_Rigidbody = GetComponentInParent<Rigidbody2D>();
        m_targetPlayer = GameObject.FindGameObjectWithTag("Player");
        InitializeState();
    }

    void FixedUpdate()
    {
        ChangeState();
        m_moveVelocity.y = m_Rigidbody.velocity.y;
        m_Rigidbody.velocity = m_moveVelocity;
    }

    private void InitializeState()
    {
        enemyState = SmallEnemyState.Patrol;
        m_originalState = enemyState;
        m_patrolStart = transform.position + Vector3.left * patrolRadius;
        m_patrolEnd = transform.position + Vector3.right * patrolRadius;
        m_patrolDuration = (m_patrolEnd - m_patrolStart).magnitude / speed;
        m_direction = 1;
    }

    private void OnDrawGizmos()
    {
        Gizmos.color = Color.red;
        Gizmos.DrawWireSphere(transform.position, attackRadius);
        Gizmos.color = Color.yellow;
        Gizmos.DrawWireSphere(transform.position, searchRadius);
        Gizmos.color = Color.green;
        Gizmos.DrawWireSphere(transform.position, loseTargetRadius);
        Gizmos.color = Color.black;
    }

    private void ChangeState()
    {
        switch (enemyState)
        {
            case SmallEnemyState.Idle:
                //EnemyAnim.SetBool("isAttacking", false);
                Search();
                break;
            case SmallEnemyState.Patrol:
                //EnemyAnim.SetBool("isAttacking", false);
                Patrol();
                Search();
                break;
            case SmallEnemyState.Attack:
                //EnemyAnim.SetBool("isAttacking", true);
                Attack();
                break;
            case SmallEnemyState.Chase:
                //EnemyAnim.SetBool("isAttacking", false);
                Chase();
                break;
            default:
                break;
        }
    }

    bool isGround(float offsetX, float rayLength)
    {
        Vector2 rayStart = new Vector2(m_Rigidbody.transform.position.x + offsetX, m_Rigidbody.transform.position.y);
        Debug.DrawRay(rayStart, Vector2.down * rayLength);//???????
        RaycastHit2D ray = Physics2D.Raycast(rayStart, Vector2.down, rayLength);
        if (ray.collider != null)
        {
            return true;
        }
        else
            return false;
    }

    private void EnemyFacing()
    {
        float distanceSub = m_targetPlayer.transform.position.x - transform.position.x;
        if (distanceSub < 0)
        {
            transform.GetComponent<Rigidbody2D>().transform.localScale = new Vector3(1f, 1f, 1f);
        }
        if (distanceSub > 0)
        {
            transform.GetComponent<Rigidbody2D>().transform.localScale = new Vector3(-1f, 1f, 1f);
        }
    }

    private void StartChasingPlayer()
    {
        m_moveVelocity.x = (m_targetPlayer.transform.position - transform.parent.position).normalized.x * chaseSpeed;
    }

    void Chase()
    {
        if (m_targetPlayer)
        {
            EnemyFacing();
            float distance = Vector2.Distance(m_targetPlayer.transform.position, m_Rigidbody.transform.position);
            if (distance <= attackRadius)
            {
                enemyState = SmallEnemyState.Attack;
            }
            else if (distance >= loseTargetRadius)
            {
                BackToOriginalState();
            }
            else
            {
                StartChasingPlayer();
            }
        }
    }

    void Search()
    {
        if (m_targetPlayer)
        {
            float distance = Vector2.Distance(m_targetPlayer.transform.position, m_Rigidbody.transform.position);
            if (distance <= loseTargetRadius)
            {
                enemyState = SmallEnemyState.Chase;
            }
            m_moveVelocity.x = 0f;
        }
    }

    void Patrol()
    {
        if (!isGround(xOffSet, rayLength))
        {
            m_direction = -1;
            transform.GetComponent<Rigidbody2D>().transform.localScale = new Vector3(1f, 1f, 1f);
        }
        else if (!isGround(-xOffSet, rayLength))
        {
            m_direction = 1;
            transform.GetComponent<Rigidbody2D>().transform.localScale = new Vector3(-1f, 1f, 1f);
        }
        m_Rigidbody.velocity = new Vector2(m_direction * speed, 0);
    }

    void Attack()
    {
        EnemyFacing();
        m_moveVelocity.x = 0;
        float distance = Vector2.Distance(m_targetPlayer.transform.position, transform.position);
        if (distance >= attackRadius)
        {
            enemyState = SmallEnemyState.Chase;
        }

        if (Input.GetKeyDown(KeyCode.E))
        {
            if (enemyAnim != null)
                enemyAnim.SetTrigger("Attack");
        }
    }

    private void BackToOriginalState()
    {
        enemyState = m_originalState;
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        //be attack by the player
        if (collision.tag == "weapon" && (this.gameObject.transform.position.x - FindOwner(collision.gameObject).transform.position.x) * FindOwner(collision.gameObject).transform.localScale.x > 0)
        {
            Debug.Log("hit" + this.name);
            m_SpriteRenderer.color = Color.red;
            Invoke("Recover", 0.2f);
            // freeze frame
            collision.GetComponentInParent<Player>().canFreeze = true;
        }
    }
    private void OnTriggerExit2D(Collider2D collision)
    {
        if (collision.tag == "weapon")
        {
            collision.GetComponentInParent<Player>().canFreeze = false;
        }
    }

    private void Recover()
    {
        m_SpriteRenderer.color = Color.green;
    }

    GameObject FindOwner(GameObject weapon)
    {
        GameObject Owner = weapon;
        while (Owner.transform.parent != null)
            Owner = Owner.transform.parent.gameObject;
        return Owner;
    }

    #region AnimationEvent
    void ActiveCollider()
    {
        weaponCollider.SetActive(true);
    }
    void InActiveCollider()
    {
        weaponCollider.SetActive(false);
    }
    #endregion
}