using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Puppet_Weapon : MonoBehaviour
{
    public PuppetLogic PuppetOwner;

    private void Start()
    {
        PuppetOwner = GetComponentInParent<PuppetLogic>();
    }
    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.tag == "Player")
        {
            collision.GetComponentInChildren<Player>().BeHurt(this.gameObject, PuppetOwner.hurtFrame, PuppetOwner.hurtForce);
        }
    }
}
