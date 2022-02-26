using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Enemy_weapon_test : MonoBehaviour
{
    [HideInInspector]
    public GameObject Owner;// the owner of this weapon
    private void Start()
    {
        if(GetComponentInParent<Enemy>())
            Owner = GetComponentInParent<Enemy>().gameObject;
        else if(GetComponentInParent<PuppetLogic>())
            Owner = GetComponentInParent<PuppetLogic>().gameObject;
        //Debug.Log(Owner.gameObject.name);
    }
    private void OnTriggerEnter2D(Collider2D collision)
    {
        if(collision.tag=="Player")
        {
            //Debug.Log("hurt");
            if(GetComponentInParent<Enemy>())
                collision. GetComponentInChildren<Player>().BeHurt(this.gameObject, GetComponentInParent<Enemy>().hurtFrame, GetComponentInParent<Enemy>().hurtForce);
            else if(GetComponentInParent<PuppetLogic>())
                collision.GetComponentInChildren<Player>().BeHurt(this.gameObject, GetComponentInParent<PuppetLogic>().hurtFrame, GetComponentInParent<PuppetLogic>().hurtForce);
        }
    }
}
