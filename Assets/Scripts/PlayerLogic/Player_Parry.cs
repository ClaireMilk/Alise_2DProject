using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public partial class Player : MonoBehaviour
{
    #region Variable
    public static bool canPerfectParry;
    [Header("Parry")]
    public GameObject collider_Parry;
    public GameObject collider_Parry_Down;
    public float perfectParryFrame;
    public float downParryHeight_perfect;
    public float downParryHeight_nonPerfect;
    public GameObject perfectParry_vfx;
    public GameObject nonPerfectParry_vfx;
    public float perfectBackOffDis;
    public float backForce;


    GameObject parryObj;
    int perfectParryTimes;
    bool isDownParry;
    bool isDownTrigger;
    #endregion

    public void Parry()
    {
        if (Input.GetButtonDown("Parry")&&currentState!=PlayerState.Jump&& currentState != PlayerState.ParryJump)
        {
            collider_Parry.GetComponent<Collider2D>().enabled = true;
            animator.SetBool("Parry", true);
            canInput = false;
            StartCoroutine(PerfectFrame());
            currentState = PlayerState.Parry;
        }

        if (Input.GetButtonUp("Parry")&&currentState!=PlayerState.ParryJump)
        {
          //  Debug.Log("normalparry");
            animator.SetBool("Parry", false);
            CloseCollider();
            //Invoke("CloseCollider", 0.1f);

        }
        if (Input.GetButtonDown("Parry") && !isDownTrigger)
        {
            //Debug.Log("11");
            isDownParry = true;
            isDownTrigger = true;
        }
        if(Input.GetButtonUp("Parry"))
        {
           // Debug.Log("downparry");
            isDownTrigger = false;
        }
    }
    void DownParry()
    {
        Debug.Log("ÏÂ¸ñµ²");
        collider_Parry_Down.GetComponent<Collider2D>().enabled = true;
        // animator.SetBool("ParryDown", true);
        StartCoroutine(PerfectFrame());
        //currentState = PlayerState.ParryJump;
    }
    public void CloseCollider()
    {
        collider_Parry.GetComponent<Collider2D>().enabled = false;
        collider_Parry_Down.GetComponent<Collider2D>().enabled = false;
        if(currentState !=PlayerState.Execute)
             currentState = PlayerState.Idle;
        canInput = true;
    }
    IEnumerator PerfectFrame()
    {
        canPerfectParry = true;
        yield return new WaitForSecondsRealtime(perfectParryFrame * Time.deltaTime);
        canPerfectParry = false;
    }
    public void PerfectParry(GameObject enemy)
    {
        if(enemy.GetComponentInParent<Enemy>())
            parryObj = enemy.GetComponentInParent<Enemy>().gameObject;
        else if(enemy.GetComponentInParent<PuppetLogic>())
            parryObj = enemy.GetComponentInParent<PuppetLogic>().gameObject;
        if(enemy.GetComponent<PolygonCollider2D>())
        {
            enemy.GetComponent<PolygonCollider2D>().enabled = false;
            perfectParryTimes++;
            //Debug.Log(perfectParryTimes);
            parryObj.GetComponent<Animator>().SetTrigger("BeParried");
            if (parryObj.GetComponent<Enemy>() && parryObj.GetComponent<Enemy>(). enemyAttack == EnemyAttack.DownAttack)
            {
              //  Debug.Log(new Vector2((-transform.parent.position.x + parryObj.GetComponentInParent<Enemy>().transform.position.x) /
   // Mathf.Abs(-transform.parent.position.x + parryObj.GetComponentInParent<Enemy>().transform.position.x) * perfectBackOffDis, 0));
                selfRigidbody.AddForce(new Vector2((-transform.parent. position.x + parryObj.GetComponentInParent<Enemy>(). transform.position.x) /
    Mathf.Abs(-transform.parent.position.x + parryObj.GetComponentInParent<Enemy>().transform.position.x) * perfectBackOffDis, 0));
                animator.SetBool("Parry", false);
                animator.SetTrigger("BackOff");
                // currentState = PlayerState.Hurt;
            }
            else
            {
                BackOff(parryObj, backForce);
            }
        }

        // ParryEffect();
        //Debug.Log("PerfectParry");
       // canFreeze = true;
        PlaySoundEffect(PlayerAction.ParryPerfect_1);
        perfectParry_vfx.GetComponent<ParticleSystem>().Play();
    }
    public void NonPerfectParry(GameObject enemy)
    {
        // perfectParryTimes = 0;
        if (enemy.GetComponentInParent<Enemy>())
            parryObj = enemy.GetComponentInParent<Enemy>().gameObject;
        else if (enemy.GetComponentInParent<PuppetLogic>())
            parryObj = enemy.GetComponentInParent<PuppetLogic>().gameObject;
        if(enemy.GetComponent<PolygonCollider2D>())
        {
            enemy.GetComponent<PolygonCollider2D>().enabled = false;
            BackOff(parryObj, backForce);
        }
        // Debug.Log("noPerfectParry");
        PlaySoundEffect(PlayerAction.ParryImperfect_1);                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                          
        nonPerfectParry_vfx.GetComponent<ParticleSystem>().Play();
    }
    public void PerfectParry_Down(GameObject enemy)
    {
       // Debug.Log("PerfectParry");
        PlaySoundEffect(PlayerAction.ParryPerfect_1);
         selfRigidbody.velocity = new Vector2(selfRigidbody.velocity.x, downParryHeight_perfect);
        //selfRigidbody.AddForce(new Vector2(0, downParryHeight_perfect),ForceMode2D.Impulse);
    }
    public void NonPerfectParry_Down(GameObject enemy)
    {
        Debug.Log("noPerfectParry");
        PlaySoundEffect(PlayerAction.ParryImperfect_1);
        selfRigidbody.velocity = new Vector2(selfRigidbody.velocity.x, downParryHeight_nonPerfect);
    }
    public void ParryEffect(GameObject collider)
    {
        collider.GetComponent<Animator>().SetTrigger("PerfectEffect");
    }
    void BackOff(GameObject ParryObject, float backForce)
    {
        float direction = (-transform.parent.position.x + ParryObject.transform.position.x) /
                   Mathf.Abs(transform.parent.position.x - ParryObject.transform.position.x);
        //add force to player
        selfRigidbody.AddForce(new Vector2(direction * backForce, 0));
    }
}
