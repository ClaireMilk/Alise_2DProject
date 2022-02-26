using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ParryCollider : MonoBehaviour
{
    Player Owner;
    public bool isDownParryCollider;
    public GamepadVibrationComponent GamepadVibration;
    private void Awake()
    {
        Owner = GetComponentInParent<Player>();
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (isDownParryCollider != true && collision.gameObject.tag == "enemyWeapon" && (collision.GetComponent<Enemy_weapon_test>().Owner.transform.position.x - Owner.transform.parent.position.x) * Owner.transform.parent.localScale.x > 0)
        {
            if (Player.canPerfectParry)
            {
                Owner.PerfectParry(collision.gameObject);
                GamepadVibration.ParrySmallVibration();
            }
            else
            {
                Owner.NonPerfectParry(collision.gameObject);
                GamepadVibration.ParrySmallVibration();
            }
            // this.GetComponent<Collider2D>().enabled = false;
        }
        if (isDownParryCollider == true)
        {
            //Debug.Log(collision.gameObject.name);
            if (collision.gameObject.tag == "Thorn_Parry")
            {
                Owner.currentState = Player.PlayerState.ParryJump;
                Owner.animator.SetTrigger("ParryDown_t");
                if (Player.canPerfectParry)
                {
                    //Debug.Log("per");
                    Owner.PerfectParry_Down(collision.gameObject);
                    GamepadVibration.ParrySmallVibration();
                }
                else
                {
                    //Debug.Log("non");
                    Owner.NonPerfectParry_Down(collision.gameObject);
                    GamepadVibration.ParrySmallVibration();
                }
            }
            //Prohibit disable the collider by mistake 
            if (collision.gameObject.tag != "Player" && collision.gameObject.layer != 8)
                this.GetComponent<Collider2D>().enabled = false;
        }
    }

}
