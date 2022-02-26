using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public partial class Player : MonoBehaviour
{
    #region variable
    bool isAttacking;
    bool isStoreStage;
    bool attackTwice;
    int attackTimes;
    [Header("Attack")]
    public GameObject weaponCollider;
    public GameObject attack_VFX;
    public int attackDamage;
    public int ExecuteDamage;

    bool once;
    bool twice;
    bool third;
    #endregion

    /// <summary>
    /// Player attack
    /// </summary>  
    //play the animation
    //before the first event, you can attack 
    void PlayerAttack()
    {
        attackTimes++;
        // Debug.Log(attackTimes);
        if (attackTimes % 3 == 2)
        {
            animator.SetTrigger("AttackTwice");
            once = false;
            twice = true;
            third = false;
            //PlaySoundEffect(PlayerAction.AttackHit_2);

        }
        else if (attackTimes % 3 == 0)
        {
            animator.SetTrigger("Attack3");
            once = false;
            twice = false;
            third = true;
            // PlaySoundEffect(PlayerAction.AttackHit_3);
        }
        else
        {
            animator.SetTrigger("Attack");
            once = true;
            twice = false;
            third = false;
        }

        currentState = PlayerState.PrepareAttack;
        //play the sfx and vfx
    }
    public void FreezeFrame()
    {
        _pendingFreezeDuration = duration;
    }
    IEnumerator DoFreeze()
    {
        _isFrozen = true;
        var original = Time.timeScale;
        Time.timeScale = 0f;
        yield return new WaitForSecondsRealtime(duration);
        Time.timeScale = original;
        _pendingFreezeDuration = 0;
        _isFrozen = false;
    }

    #region AnimationEvent
    // Before this event, parry can cancel the attack
    //After this event, any input is invalid
    void FreezeEvent()
    {
        // Debug.Log("freeze");
        canInput = false;
        attack_VFX.SetActive(true);
    }
    //After this event, store player's input
    void StoreInputEvent()
    {
        isStoreStage = true;
    }
    void Freeze()
    {
        if (canFreeze == true)
        {
            FreezeFrame();
            canFreeze = false;
        }

    }
    //active the weapon collider, enter the attack period
    void AttackEvent()
    {
        currentState = PlayerState.Attack;
        weaponCollider.GetComponent<PolygonCollider2D>().enabled = true;
        weaponCollider.GetComponent<CircleCollider2D>().enabled = true;
    }
    //inactive the weapon collider, enter the recover period, can input
    void RecoverEvent()
    {
        currentState = PlayerState.RecoverAttack;
        weaponCollider.GetComponent<PolygonCollider2D>().enabled = false;
        weaponCollider.GetComponent<CircleCollider2D>().enabled = false;
        canInput = true;
    }
    void StopStoreInput()
    {
        isStoreStage = false;
        if (attackTwice == true)
        {
            //Debug.Log("twice");
            //sr.color = Color.red;
            isAttacking = true;
            attackTwice = false;
        }
    }
    void AttackEnd()
    {
        currentState = PlayerState.Idle;
        // sr.color = Color.white;
        attack_VFX.SetActive(false);
    }
    void PlayAttackSound()
    {
        if (weaponCollider.GetComponent<AttackCollider>().isAttackEnemy)
        {
            if (once)
                PlaySoundEffect(Player.PlayerAction.AttackHit_1);
            else if (twice)
                PlaySoundEffect(Player.PlayerAction.AttackHit_2);
            else if (third)
                PlaySoundEffect(Player.PlayerAction.AttackHit_3);
        }
        else
            PlaySoundEffect(Player.PlayerAction.AttackMiss);

        //reset bool
        weaponCollider.GetComponent<AttackCollider>().isAttackEnemy = false;
        once = false;
        twice = false;
        third = false;
    }
    #endregion
}
