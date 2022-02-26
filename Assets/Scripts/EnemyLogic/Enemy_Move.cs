using System.Collections;
using System.Collections.Generic;
using UnityEngine;


public enum EnemyState
{
    Idle,
    Patrol,
    Attack,
    Chase,
    BeExecuted
}

public partial class Enemy : MonoBehaviour
{
    [Header("Movement")]
    public Transform eye;
    public float speed;
    public float chaseSpeed;
    public float rayLength;


    public float patrolRadius;
   // public float searchRadius;
    public float loseTargetRadius;
    public float attackRadius;



    public EnemyState enemyState;

    private EnemyState m_originalState;
    private GameObject m_targetPlayer;
    private float m_direction;

    float m_attackTimer;

    Vector2 m_patrolStart;
    Vector2 m_patrolEnd;

    float m_patrolDuration;

    [HideInInspector]
    public Vector2 m_moveVelocity;
    //Vector2 m_ChaseVelocity;

    /// <summary>
    /// Set the state
    /// </summary>
    private void ChangeState()
    {
        switch (enemyState)
        {
            case EnemyState.Idle:
                EnemyAnim.SetBool("IsAttacking", false);
                Search();
                break;
            case EnemyState.Patrol:
                EnemyAnim.SetBool("IsAttacking", false);
                Patrol();
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

    #region EnemyState
    void Patrol()
    {
        //float t = Mathf.InverseLerp(0, m_patrolDuration, Mathf.PingPong(Time.time, m_patrolDuration));
        //Vector2 position = Vector2.Lerp(m_patrolStart, m_patrolEnd, t);
        //m_moveVelocity.x = (position - (Vector2)transform.position).normalized.x * speed;
        //if(m_moveVelocity.x<0)
        //{
        //    transform.rotation = Quaternion.Euler(0, 0, 0);
        //}
        //if(m_moveVelocity.x>0)
        //{
        //    transform.rotation = Quaternion.Euler(0, 180.0f, 0);
        //}
        m_moveVelocity.x = m_direction * speed;
    }
    void Search()
    {
        if (m_targetPlayer)
        {
            float distance = Vector2.Distance(m_targetPlayer.transform.position, m_Rigidbody.transform.position);
            if (distance <= loseTargetRadius)
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

    #endregion


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

    /// <summary>
    /// face the player
    /// </summary>
    private void EnemyFacing()
    {
        float distanceSub = m_targetPlayer.transform.position.x - transform.position.x;
        if (distanceSub < 0)
        {
            transform.GetComponentInParent<Rigidbody2D>().transform.localScale = new Vector3(0.4f, 0.4f, 0.4f);
        }
        if (distanceSub > 0)
        {
            transform.GetComponentInParent<Rigidbody2D>().transform.localScale = new Vector3(-0.4f, 0.4f, 0.4f);
           // Debug.Log("Fuckyou");
        }
    }
    void AvoidOutside()
    {
        GameObject collider;
        // Debug.Log(isFaceWall(eye.position, rayLength));
        if (isFaceWall(eye.position,rayLength,out collider))
        {
            if (enemyAttack!=EnemyAttack.Wander)
            {
                m_moveVelocity.x = 0f;
            }
            else
            {
                m_direction = (this.transform.position.x - collider.transform.position.x) / Mathf.Abs(this.transform.position.x - collider.transform.position.x);
            }
        }
    }
    /// <summary>
    /// Try to avoid enemy push player
    /// </summary>
    void AvoidCollidePlayer()
    {
        GameObject collider;
    
        if (isFaceWall(eye.position, rayLength, out collider))
        {
            
            if (enemyAttack != EnemyAttack.Wander)
            {
               // Debug.Log("enter1");
                m_moveVelocity.x = 0f;
            }
            else if(Mathf.Abs(this.transform.position.x - m_targetPlayer.transform.position.x) >= downDistance)
            {
                //Debug.Log("enter2");
                m_direction = (this.transform.position.x - collider.transform.position.x) / Mathf.Abs(this.transform.position.x - collider.transform.position.x);
            }
            else if((this.transform.position.x - collider.transform.position.x)* (this.transform.position.x - m_targetPlayer.transform.position.x)>0)
            {
                m_direction = (this.transform.position.x - collider.transform.position.x) / Mathf.Abs(this.transform.position.x - collider.transform.position.x);
            }
            else
            {
                //Debug.Log("enter3");
                m_direction = 0;
            }
        }
        else if(Mathf.Abs(this.transform.position.x - m_targetPlayer.transform.position.x) < downDistance)
        {
            m_direction = (this.transform.position.x - m_targetPlayer.transform.position.x) / Mathf.Abs(this.transform.position.x - m_targetPlayer.transform.position.x);
        }
    }
   bool isFaceWall(Vector2 eye,  float rayLength,out GameObject collider)
    {
        Vector2 rayStart = eye;
        Debug.DrawRay(rayStart, Vector2.right * rayLength);//将射线显示出来
        Debug.DrawRay(rayStart, -Vector2.right * rayLength);//将射线显示出来
        RaycastHit2D ray1 = Physics2D.Raycast(rayStart, Vector2.right, rayLength, 1 << 8);
        RaycastHit2D ray2 = Physics2D.Raycast(rayStart, -Vector2.right, rayLength, 1 << 8);
        if ((ray1.collider!=null&& ray1.collider.tag == "Wall")||( ray2.collider != null&&ray2.collider.tag == "Wall"))
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

}
