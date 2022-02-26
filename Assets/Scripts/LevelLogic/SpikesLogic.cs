using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SpikesLogic : MonoBehaviour
{
    Player m_playerLogic;
    public int attackDamage;

    float timer;
    float interval;
    private void Start()
    {
        m_playerLogic = GameObject.FindGameObjectWithTag("Player").GetComponentInChildren<Player>();
        timer = 0;
        interval = 1;
    }
    private void OnTriggerStay2D(Collider2D collision)
    {
        if (collision.gameObject.tag == "Player")
        {
            m_playerLogic.BeHurt(this.gameObject, 10);
        }
    }
}
