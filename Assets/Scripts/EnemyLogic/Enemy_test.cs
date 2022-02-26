using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Enemy_obsolete_test : MonoBehaviour
{
    public GameObject weaponCollider;
    public int maxHP;

    int currentHP;
    Animator enemyAnim;
    SpriteRenderer sr;
    private void Awake()
    {
        enemyAnim = GetComponent<Animator>();
        sr = GetComponent<SpriteRenderer>();
        weaponCollider.SetActive(false);
        currentHP = maxHP;
    }
    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.E))
        {
            if (enemyAnim != null)
                enemyAnim.SetTrigger("Attack");
        }
    }
    //Freeze the frame
    private void OnTriggerEnter2D(Collider2D collision)
    {
        //be attack by the player
          if (collision.tag == "weapon"&&(this.gameObject.transform.position.x-FindOwner(collision.gameObject).transform.position.x)* FindOwner(collision.gameObject).transform.localScale.x > 0)
            {
                Debug.Log("hit"+this.name);
                 sr.color = Color.red;
                Invoke("Recover", 0.2f);
            // freeze frame
            collision.GetComponentInParent<Player>().canFreeze = true;
            }
    }
    private void OnTriggerExit2D(Collider2D collision)
    {
        if (collision.tag == "weapon")
        {
            collision.GetComponentInParent<Player>().canFreeze = false;
        }
    }

    private void Recover()
    {
        sr.color = Color.green;
    }

    GameObject FindOwner(GameObject weapon)
    {
        GameObject Owner = weapon;
        while (Owner.transform.parent != null)
            Owner = Owner.transform.parent.gameObject;
        return Owner;
    }

    #region AnimationEvent
    void ActiveCollider()
    {
        weaponCollider.SetActive(true);
    }
    void InActiveCollider()
    {
        weaponCollider.SetActive(false);
    }
    #endregion
}
