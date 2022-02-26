using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SmallEnemy_Weapon : MonoBehaviour
{
    [HideInInspector]
    public SmallEnemy Owner;// the owner of this weapon

    private void Start()
    {
        Owner = GetComponentInParent<SmallEnemy>();
        //Debug.Log(Owner.gameObject.name);
    }
    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.tag == "Player")
        {
            collision.GetComponentInChildren<Player>().BeHurt(this.gameObject, Owner.hurtFrame, Owner.hurtForce);
        }
    }
}
