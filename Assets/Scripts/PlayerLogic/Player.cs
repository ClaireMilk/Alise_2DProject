using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public partial class Player : MonoBehaviour
{
    #region variable
    [HideInInspector]
    public Animator animator;
    [HideInInspector]
    public Rigidbody2D selfRigidbody;
    //Variable for Freeze
    [HideInInspector]
    public bool canFreeze;
    bool _isFrozen = false;
    float _pendingFreezeDuration = 0f;
    [Header("FreezeFrame")]
    [Range(0, 1f)]
    public float duration = 1f;
    [Header("Basic")]
    public GameObject groundPoint;//the start point of raycast
    [HideInInspector]
    public bool canInput;//if player can input
    SpriteRenderer sr;
    Transform trans;
    public enum PlayerState
    {
        Idle,
        Jump,
        PrepareAttack,
        Attack,
        RecoverAttack,
        Hurt,
        Parry,
        Execute,
        ParryJump
    }
    public PlayerState currentState;

    public int maxHP;
    public int currentHP;
    #endregion


    private void Awake()
    {
        currentHP = PlayerPrefs.GetInt("currentHP");

        //allow the input
        canInput = true;
        //HP
        int x = SceneManager.GetActiveScene().buildIndex;
        if (currentHP <= 0 || x == 1)
            currentHP = maxHP;
        //set the frame 30 frame/s
        Application.targetFrameRate = 60;
        //obtain
        animator = GetComponent<Animator>();
        selfRigidbody = groundPoint.GetComponent<Rigidbody2D>();
        trans = groundPoint.GetComponent<Transform>();
        sr = GetComponentInChildren<SpriteRenderer>();

        //InitiateAudio
        InitializeAudioSources();

        //FreezeFramebool Initiate
        canFreeze = false;

        //Innitiate state
        currentState = PlayerState.Idle;

        //Initiate Attack
        weaponCollider.GetComponent<PolygonCollider2D>().enabled = false;
        weaponCollider.GetComponent<CircleCollider2D>().enabled = false;
        attack_VFX.SetActive(false);
        isStoreStage = false;
        attackTwice = false;
        attackTimes = 0;
        //Initiate Parry
        collider_Parry.GetComponent<Collider2D>().enabled = false;
        collider_Parry_Down.GetComponent<Collider2D>().enabled = false;
        perfectParryTimes = 0;
        isDownParry = false;
        isDownTrigger = false;
        //Initiate Run
        originalTransform = transform;
        //Initiate Execute
        canExecute = false;
        isExecuting = false;
        isFlying = false;
        execution_vfx.SetActive(false);
        //Initiate Jump
        isJump = false;
        //Iniatiate UI
        if (playerhealthbar != null)
            playerhealthbar.value = 1f;
        //Initiate hurt
        timer = interval;
        //pos
        // transform.position = GameManager.Instance.lastPosition;
    }
    private void Update()
    {
        //Debug.Log(animator.GetBool("Parry"));
        //Debug.Log(currentState);
        //  Debug.Log("canInput"+canInput);
        // Debug.Log(xInput);
        //    Debug.Log(currentState);
        //Update HP
        // ResetTimer();
        if (playerhealthbar != null)
            playerhealthbar.value = currentHP * 1.0f / maxHP;
        if (currentHP <= 0)
        {
            Debug.Log("die");
            PlayerDie();
            //SceneManager.LoadScene(0);
        }
        if (currentState == PlayerState.Idle || currentState == PlayerState.Jump)
        {
            canInput = true;
            isStoreStage = false;
            animator.ResetTrigger("ParryDown_t");
            weaponCollider.GetComponent<PolygonCollider2D>().enabled = false;
            weaponCollider.GetComponent<CircleCollider2D>().enabled = false;
        }
        if (currentState == PlayerState.Hurt || currentState == PlayerState.Parry || currentState == PlayerState.Execute)
        {
            xInput = 0;
        }

        //Input for Parry
        if (currentState != PlayerState.Hurt && currentState != PlayerState.Execute)
            Parry();
        if (canInput == true)
        {
            //Input for move
            if (currentState != PlayerState.PrepareAttack && currentState != PlayerState.Attack && currentState != PlayerState.RecoverAttack)        //player cannot move at the attack period
                xInput = Input.GetAxisRaw("Horizontal");
            else
                xInput = 0;
            //Input for attack
            if (isStoreStage != true && Input.GetButtonDown("Attack") && currentState != PlayerState.PrepareAttack)//in the prepareattack state, player can not cancel attack
            {
                if (canExecute == true)
                {
                    Time.timeScale = 1;
                    isExecuting = true;
                    canExecute = false;
                }
                else
                {
                    isAttacking = true;
                }
                attackTimes = 0;
            }
            //Input for Jump
            //detect whether player is on the ground, prevent jump more than once
            if ((IsGround(xOffSet, rayLength) || IsGround(-xOffSet, rayLength)))
            {
                selfRigidbody.velocity = new Vector2(selfRigidbody.velocity.x, 0);
                //触地可以跳跃，以下为跳跃代码
                if (Input.GetButtonDown("Jump"))
                {
                    isJumping = true;
                }
            }
        }
        //store the attack input and attack after the attack period end
        if (isStoreStage == true)
        {
            if (Input.GetButtonDown("Attack"))
            {
                attackTwice = true;
            }
        }
        //Freeze
        if (_pendingFreezeDuration != 0 && !_isFrozen)
        {
            //StartCoroutine(DoFreeze());
        }
        Execute();
    }
    private void FixedUpdate()
    {
        PlayerMove(xInput);
        Fraction();
        BetterJumpFelling();

        animator.SetBool("isGrounded", IsGround(xOffSet, rayLength) || IsGround(-xOffSet, rayLength));
        if (isExecuting == true)
        {
            isExecuting = false;
            PlayerExecute();
        }
        else if (isAttacking == true)
        {
            isAttacking = false;
            PlayerAttack();
        }
        if (isDownParry == true)
        {
            isDownParry = false;
            if ((currentState == PlayerState.Jump || currentState == PlayerState.ParryJump) && animator.GetBool("Fall"))
                DownParry();
        }
        Jump();
        SetJumpState();
    }

    public void HealthBottleHeal(int count, GameObject pickup, AudioClip Sound)
    {
        if (currentHP != maxHP)
        {
            currentHP += count;
            if (currentHP > maxHP)
            {
                currentHP = maxHP;
            }
            PlaySoundName(Sound);
            Destroy(pickup);
        }
    }
    public void RecoverAnimatorParameter()
    {
        animator.ResetTrigger("BackOff");
        animator.SetBool("BeHurt", false);
        animator.SetBool("Parry", false);
        animator.SetBool("Fall", false);
        animator.ResetTrigger("ParryDown");
        animator.ResetTrigger("ParryDown_t");
        animator.ResetTrigger("ExecuteEnd");
        animator.ResetTrigger("Execute");
        animator.ResetTrigger("Attack3");
        animator.ResetTrigger("Hurt");
        animator.ResetTrigger("Jump");
        animator.ResetTrigger("AttackTwice");
        animator.ResetTrigger("Attack");
    }

    private void OnDestroy()
    {
        PlayerPrefs.SetInt("currentHP", currentHP);
    }

    public void RecoverInput()
    {
        canInput = true;
        currentState = PlayerState.Idle;
        isAttacking = false;
    }
}

