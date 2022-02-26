using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public partial class Player : MonoBehaviour
{
    [Header("Hurt")]
    // public float hurtForce;
    public float interval;
    public float timer;
    float lastTime;
    public void BeHurt(GameObject attackObj,int hurtFrame, float hurtForce)
    {
        selfRigidbody.velocity = new Vector2(selfRigidbody.velocity.x,0);
        StartCoroutine(BeHurt_Ienum(attackObj,hurtFrame,hurtForce));
    }
    public void BeHurt(GameObject attackObj,int hurtFrame)
    {
        if (timer < interval)
        {
            timer += Time.deltaTime;
           // Debug.Log(timer);
        }
        else
        {
            timer = 0;
            StartCoroutine(BeHurt_Ienum(attackObj, hurtFrame, 0));
        }

    }
    void ResetTimer()
    {
       // Debug.Log(timer);
        if(timer==lastTime)
        {
            timer = interval;
        }
        lastTime = timer;
    }
    IEnumerator BeHurt_Ienum(GameObject attackObj,int hurtFrame,float hurtForce)
    {
        if (currentState != PlayerState.Parry||( currentState == PlayerState.Parry&&(attackObj.GetComponent<Enemy_weapon_test>())&&(attackObj.GetComponent<Enemy_weapon_test>().Owner. transform.position.x  - transform.parent.position.x) * transform.parent.localScale.x < 0))
        {
            weaponCollider.GetComponent<PolygonCollider2D>().enabled = false;
            weaponCollider.GetComponent<CircleCollider2D>().enabled = false;
            canInput = false;
           // Debug.Log("hurt");
            animator.SetBool("Parry", false);
            //animator.Play("Player_Hurt_0");
            animator.SetBool("BeHurt", true);
            canInput = false;
            PlaySoundEffect(PlayerAction.Hurt_1);
            currentState = PlayerState.Hurt;
            GamepadVibration.ParrySmallVibration();
            if (attackObj.GetComponent<Enemy_weapon_test>()!=null)
            {
                float direction = (-transform.parent.position.x + attackObj.GetComponent<Enemy_weapon_test>().Owner.transform.position.x) /
                    Mathf.Abs(transform.parent.position.x - attackObj.GetComponent<Enemy_weapon_test>().Owner. transform. position.x);
                //add force to player
               // Debug.Log(new Vector2(direction * hurtForce, 0));
                selfRigidbody.AddForce(new Vector2(direction * hurtForce, 0));
                //selfRigidbody.AddForce(new Vector2(direction * hurtForce, -direction * hurtForce/5));
                //lose hp
                if(attackObj.GetComponentInParent<Enemy>())
                    currentHP -= attackObj.GetComponentInParent<Enemy>().attackDamage/2;
                else if(attackObj.GetComponentInParent<PuppetLogic>())
                    currentHP -= attackObj.GetComponentInParent<PuppetLogic>().attackDamage;
            }
            else if(attackObj.GetComponent<SpikesLogic>()!=null)
            {
                currentHP -= attackObj.GetComponent<SpikesLogic>().attackDamage;
            }
            else
            {
                currentHP -= attackObj.GetComponent<Slime>().damage;

            }
            //reset execution
            perfectParryTimes = 0;
            execution_vfx.SetActive(false);
            yield return new WaitForSeconds(Time.deltaTime*hurtFrame);
            animator.SetBool("BeHurt", false);
            canInput = true;
            currentState = PlayerState.Idle;
            // sr.color = Color.red;
            //Invoke("RecoverFromBeingHurt", 1.0f);
        }
    }
    private void RecoverFromBeingHurt()
    {
        currentState = PlayerState.Idle;
       // sr.color = Color.white;
    }
    //void DisableInput()
    //{
    //    canInput = false;
    //}

    void AbleInput()
    {
        canInput = true;
        currentState = PlayerState.Idle;
    }
    /// <summary>
    /// when player die
    /// </summary>
    void PlayerDie()
    {
        //play anim
        animator.SetTrigger("die");
        currentState = PlayerState.Hurt;
        Invoke("ReloadScene", 1.0f);
        //reload scene
    }
    void ReloadScene()
    {
        SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex);
    }
}
