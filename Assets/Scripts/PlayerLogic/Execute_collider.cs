using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;

public class Execute_collider : MonoBehaviour
{
    Player Owner;
    bool isExecuting;
    GameObject OwnerCollider;
    private void Start()
    {
        Owner = GetComponentInParent<Player>();
        OwnerCollider = Owner.gameObject.transform.parent.gameObject;
        GetComponent<CircleCollider2D>().enabled = false;
    }
    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.gameObject.layer == 8 && Owner.currentState == Player.PlayerState.Execute && isExecuting == false )
        {
           // Debug.Log("Collide");
            Owner.animator.speed = 0;
            Invoke("PlayerMove", 1.0f);
            isExecuting = true;
        }
    }
    void PlayerMove()
    {
      //  Debug.Log("PlayerMove");
        Owner.animator.SetTrigger("ExecuteEnd");
        Owner.animator.speed = 1;
        Tween tween = OwnerCollider.transform.DOMove(this.transform.position, 2f * Time.deltaTime);
        // OwnerCollider.transform.position = this.transform.position;
        GetComponent<CircleCollider2D>().enabled = false;
        isExecuting = false;
    }
}
