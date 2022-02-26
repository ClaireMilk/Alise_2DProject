using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ForbidCollideEnemy : MonoBehaviour
{
    public float backForce;
    Rigidbody2D rigid;
    bool isTooClose=false;
    private void Awake()
    {
        rigid = GetComponent<Rigidbody2D>();
    }
    private void OnTriggerEnter2D(Collider2D collision)
    {
       // Debug.Log(!isTooClose);
        if ((collision.gameObject.tag == "Enemy"&&((collision.GetComponentInChildren<Enemy>()&&
            !collision.GetComponentInChildren<Enemy>().isDisappear)|| collision.GetComponentInChildren<PuppetLogic>() || collision.GetComponentInChildren<PokerSoldierLogic>()))
            && GetComponentInChildren<Player>().currentState!=Player.PlayerState.Execute&& GetComponentInChildren<Player>().currentState != Player.PlayerState.Attack&& GetComponentInChildren<Player>().currentState != Player.PlayerState.Hurt)
        {
            GetComponentInChildren<Player>().canInput = false;
            if (GetComponentInChildren<Player>().currentState!=Player.PlayerState.Jump)
            {
                rigid.AddForce(new Vector2((-transform.position.x + collision.transform.position.x) /
                Mathf.Abs(transform.position.x - collision.transform.position.x) * backForce, -backForce/5));
                
            }
            else
            {
                rigid.AddForce(new Vector2((-transform.position.x + collision.transform.position.x) /
Mathf.Abs(transform.position.x - collision.transform.position.x) * backForce, 0));
            }
            GetComponentInChildren<Player>().currentState = Player.PlayerState.Hurt;
            //Invoke("RecoverInput", 0.5f);
        }
    }
    private void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.gameObject.layer==8&&isTooClose)
        {            
            RecoverInput();
        }
    }
    void RecoverInput()
    {
        GetComponentInChildren<Player>().canInput = true;
        GetComponentInChildren<Player>().currentState = Player.PlayerState.Idle;
    }
}
