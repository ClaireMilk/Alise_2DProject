using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;

public class AttackCollider : MonoBehaviour
{
    public GameObject weaponBottom;
    private Cinemachine.CinemachineCollisionImpulseSource MyInpulse;

    Player Owner;
    GameObject OwnerCollider;

    public GamepadVibrationComponent GamepadVibration;
    bool isExecuting;
    [HideInInspector]
    public bool isAttackEnemy;
    private void Awake()
    {
        Owner = GetComponentInParent<Player>();
        OwnerCollider = Owner.gameObject.transform.parent.gameObject;
        MyInpulse = GetComponentInParent<Cinemachine.CinemachineCollisionImpulseSource>();
        isExecuting = false;
        isAttackEnemy = false;
    }
    private void OnTriggerEnter2D(Collider2D collision)
    {
        //Debug.Log(Owner.currentState);
        if (Owner.currentState != Player.PlayerState.Execute && (collision.transform.position.x - Owner.transform.parent.position.x) * Owner.transform.parent.localScale.x > 0)
        {
            if (collision.GetComponentInChildren<Enemy>()&&!isAttackEnemy&&(!collision.GetComponentInChildren<Enemy>().isState2||(collision.GetComponentInChildren<Enemy>().isState2&& !collision.GetComponentInChildren<Enemy>().isDisappear)))
            {
                Debug.Log("enter");
                collision.GetComponentInChildren<Enemy>().EnemyCurrentHealth -= Owner.attackDamage ;
                StartCoroutine(ChangeColor(collision.gameObject));
                GamepadVibration.SmallVibration();
                GamepadVibration.FreezeFrame();
                MyInpulse.GenerateImpulse();
                isAttackEnemy = true;
            }
            else if (collision.GetComponentInChildren<PokerSoldierLogic>() && !isAttackEnemy)
            {
                collision.GetComponentInChildren<PokerSoldierLogic>().EnemyCurrentHealth -= Owner.attackDamage ;
                StartCoroutine(ChangeColor(collision.gameObject));
                GamepadVibration.SmallVibration();
                GamepadVibration.FreezeFrame();
                MyInpulse.GenerateImpulse();
                isAttackEnemy = true;
            }
            else if (collision.GetComponentInChildren<PuppetLogic>() && !isAttackEnemy)
            {
                collision.GetComponentInChildren<PuppetLogic>().EnemyCurrentHealth -= Owner.attackDamage ;
                StartCoroutine(ChangeColor(collision.gameObject));
                GamepadVibration.SmallVibration();
                GamepadVibration.FreezeFrame();
                MyInpulse.GenerateImpulse();
                isAttackEnemy = true;
            }
            else if (collision.GetComponent<Slime>() && !isAttackEnemy)
            {
                collision.GetComponent<Slime>().currentHP -= Owner.attackDamage ;
                if (collision.GetComponent<Slime>().currentHP <= 0)
                {
                    StopAllCoroutines();
                }
                // collision.GetComponent<Slime>().rb.velocity=Vector2.zero;
                StartCoroutine(ChangeColor(collision.gameObject));
                MyInpulse.GenerateImpulse();
                GamepadVibration.SmallVibration();
                StartCoroutine(StopMove(collision.gameObject));
                isAttackEnemy = true;
            }
            GetComponentInParent<Player>().canFreeze = true;
        }
        if (Owner.currentState == Player.PlayerState.Execute)
        {
            if (collision.GetComponentInChildren<Enemy>())
            {
                GamepadVibration.LargeVibration();
                collision.GetComponentInChildren<Animator>().SetTrigger("BeExecuted");
                collision.GetComponentInChildren<Enemy>().EnemyCurrentHealth -= Owner.ExecuteDamage / 2;
                StartCoroutine(ChangeColor(collision.gameObject));
                Owner.PlaySoundEffect(Player.PlayerAction.m_execute);
            }
            else if (collision.GetComponentInChildren<PuppetLogic>())
            {
                GamepadVibration.LargeVibration();
                collision.GetComponentInChildren<PuppetLogic>().EnemyCurrentHealth = 0;
                Owner.PlaySoundEffect(Player.PlayerAction.m_execute);
                // Destroy(collision.gameObject);
            }
            else if (collision.gameObject.tag == "Enemy")
            {
                GamepadVibration.LargeVibration();
                Owner.PlaySoundEffect(Player.PlayerAction.m_execute);
                Destroy(collision.gameObject);
            }
        }
        //if (collision.gameObject.layer == 8 && Owner.currentState == Player.PlayerState.Execute && isExecuting == false&&Owner.transform.position.x-collision.transform.position.x>0)
        //{
        //    Owner.animator.speed = 0;
        //    Invoke("PlayerMove", 1.0f);
        //    isExecuting = true;
        //}
    }
    void ExecuteEnd()
    {
        Owner.animator.SetTrigger("ExecuteEnd");
    }
    IEnumerator ChangeColor(GameObject enemy)
    {

        SpriteRenderer[] sr = enemy.GetComponentsInChildren<SpriteRenderer>();
        if (sr != null)
        {
            for (int i = 0; i < sr.Length; i++)
            {
                sr[i].color = Color.red;
            }
            yield return new WaitForSecondsRealtime(0.1f);
            for (int i = 0; i < sr.Length; i++)
            {
                sr[i].color = Color.white;
            }
        }

    }
    IEnumerator StopMove(GameObject enemy)
    {
        if (enemy.GetComponent<Slime>())
        {
            enemy.GetComponent<Slime>().isHurt = true;
            enemy.GetComponent<Slime>().rb.velocity = new Vector2(0, enemy.GetComponent<Slime>().rb.velocity.y);
        }
        yield return new WaitForSecondsRealtime(1f);
        if (enemy.GetComponent<Slime>())
        {
            enemy.GetComponent<Slime>().isHurt = false;
        }
    }
    //void PlayerMove()
    //{
    //    Debug.Log("PlayerMove");
    //    Owner.animator.SetTrigger("ExecuteEnd");
    //    Owner.animator.speed = 1;
    //    Tween tween = OwnerCollider.transform.DOMove(this.transform.position, 2f * Time.deltaTime);
    //    // OwnerCollider.transform.position = this.transform.position;
    //    GetComponent<PolygonCollider2D>().enabled = false;
    //    GetComponent<CircleCollider2D>().enabled = false;
    //    isExecuting = false;
    //}

}
