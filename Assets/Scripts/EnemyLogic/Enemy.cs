using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public partial class Enemy : MonoBehaviour
{
    Rigidbody2D m_Rigidbody;
    SpriteRenderer m_SpriteRenderer;
    Animator EnemyAnim;
    public int EnemyCurrentHealth;
    public int MaxHP = 100;
    // public Text EnemyHealthText;
    private int TimesOfPerfectParry = 0;
    public Collider2D m_collider;
    NormalAttactBehaviour normalAttackBehaviour;


    public ParticleSystem AttackParticle;
    public GameObject AttackLight;

    SpriteRenderer[] m_spriteRenderer;
    public SpriteRenderer SwordRenderer;
    List<Material> m_selfMaterialShader = new List<Material>();
    Material m_swordMat;
    const float M_INITIAFADEVALUE = 1.0f;
    const float M_ENDFADEVALUE = 0.0f;
    const float M_FADETIME = 1f;
    float m_fadeValue;
    bool m_isShowing;

    public delegate void BossDie();
    public static event BossDie OnBossDie;
    private void Start()
    {
        m_spriteRenderer = GetComponentsInChildren<SpriteRenderer>();
        foreach (var spriteRender in m_spriteRenderer)
        {
            m_selfMaterialShader.Add(spriteRender.material);
        }
        m_swordMat = SwordRenderer.material;
        m_Rigidbody = GetComponentInParent<Rigidbody2D>();

        m_targetPlayer = GameObject.FindGameObjectWithTag("Player");
        InitializeState();
        //UI
        if (!isState2)
            enemyhealthbar.gameObject.SetActive(false);
        EnemyCurrentHealth = MaxHP;
        m_leftSpikes = new List<GameObject>();
        environment_vfx.SetActive(false);
        AttackLight.SetActive(false);
        m_isShowing = isState2 ? false : true;

        if (isState2)
        {
            StartCoroutine(GradualChange(M_ENDFADEVALUE, M_INITIAFADEVALUE, M_FADETIME));
        }

        EnemyAnim.SetBool("IsShow", m_isShowing);
        //EnemyHealthText.GetComponent<Text>();
        //EnemyHealthText.text = "EnemyHealth:" + EnemyCurrentHealth;

    }
    private void Awake()
    {
        EnemyAnim = GetComponent<Animator>();
    }


    private void Update()
    {
        if (m_isShowing) return;
        float dis = Vector3.Distance(m_targetPlayer.transform.position, this.transform.position);
        if (dis < 20)
        {
            enemyhealthbar.gameObject.SetActive(true);
        }
        //Update HP
        enemyhealthbar.value = EnemyCurrentHealth * 1.0f / MaxHP;
        //Enemy die
        if (EnemyCurrentHealth <= 0)
        {
            StartCoroutine(GradualChange(M_INITIAFADEVALUE, M_ENDFADEVALUE, M_FADETIME));
            Invoke("EnemyDies", M_FADETIME);
            // Further development will require necessary animations that represent enemy deaths.
        }
        AvoidCollidePlayer();
        // AvoidOutside();
        if (isSpike)
        {
            SpikeAttack();
        }
        if (enemyState == EnemyState.BeExecuted)
            m_moveVelocity.x = 0;
    }

    private void FixedUpdate()
    {
        if (m_isShowing) return;
        ChangeState();
        ChangeMoveAnimation();
        //fix the y
        m_moveVelocity.y = m_Rigidbody.velocity.y;
        m_Rigidbody.velocity = m_moveVelocity;
        //attack count
        m_attackTimer += Time.deltaTime;

    }
    //Forbid collision with player
    private void OnCollisionEnter2D(Collision2D collision)
    {

    }
    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.gameObject.tag == "Player")
        {
            m_moveVelocity.x = 0f;
        }
    }
    #region function
    /// <summary>
    /// Show the range of AI
    /// </summary>
    private void OnDrawGizmos()
    {
        if (transform.parent != null)
        {
            Gizmos.color = Color.red;
            Gizmos.DrawWireSphere(transform.parent.position, attackRadius);
            // Gizmos.color = Color.yellow;
            // Gizmos.DrawWireSphere(transform.position, searchRadius);
            Gizmos.color = Color.green;
            Gizmos.DrawWireSphere(transform.parent.position, loseTargetRadius);
            Gizmos.color = Color.black;
            Gizmos.DrawWireSphere(transform.parent.position, Radius1);
            Gizmos.color = Color.cyan;
            Gizmos.DrawWireSphere(transform.parent.position, Radius2);
            Gizmos.color = Color.gray;
            Gizmos.DrawWireSphere(transform.parent.position, Radius3);
        }
    }
    /// <summary>
    /// Initialize the enemy state
    /// </summary>
    private void InitializeState()
    {
        enemyState = EnemyState.Idle;
        m_originalState = enemyState;
        m_patrolStart = transform.position + Vector3.left * patrolRadius;
        m_patrolEnd = transform.position + Vector3.right * patrolRadius;
        m_patrolDuration = (m_patrolEnd - m_patrolStart).magnitude / speed;
        canChangeState = true;
        m_direction = 1;
        enemyhealthbar.value = 1;
    }
    #endregion

    #region Event Ben 
    public void CreateAttackParticle()
    {
        AttackParticle.Play();
        AttackLight.SetActive(true);
        StartCoroutine(RemoveParticle(AttackParticle, AttackLight));
    }

    public void ShowOver()
    {
        m_isShowing = false;
        EnemyAnim.SetBool("IsShow", m_isShowing);
    }
    IEnumerator RemoveParticle(ParticleSystem particle, GameObject attackLight)
    {
        float t = .0f;
        while (t <= 0.25f)
        {
            t += Time.deltaTime;
        }
        particle.Clear();
        attackLight.SetActive(false);
        yield return null;
    }

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

    public void ShowTime()
    {
        if (!m_isShowing) return;
        EnemyAnim.SetTrigger("IsShow");
    }

    public void InitializeSword()
    {
        StartCoroutine(InitializeSpare(M_ENDFADEVALUE, M_INITIAFADEVALUE, M_FADETIME));
    }
    IEnumerator InitializeSpare(float initialValue, float endValue, float seconds)
    {
        Vector2 initialPos = new Vector2(0, initialValue);
        Vector2 endPos = new Vector2(seconds, endValue);
        float t = 0f;
        while (t <= 1f)
        {
            t += Time.deltaTime / seconds;
            m_fadeValue = Vector2.Lerp(initialPos, endPos, t).y;
            m_swordMat.SetFloat("_Fade", m_fadeValue);
            yield return null;
        }
    }
    #endregion
}
