using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class L1_Tutorial : MonoBehaviour
{
    private Animator anim;
    private SpriteRenderer sr;
    private bool buttonisShow;
    public GameObject arrow;

    void Start()
    {
        anim = gameObject.GetComponent<Animator>();
        sr = gameObject.GetComponent<SpriteRenderer>();
    }

    // Update is called once per frame
    void Update()
    {
        if (!buttonisShow)
        {
            sr.enabled = false;
            Destroy(arrow);
        }
        else
        {
            sr.enabled = true;
        }
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.GetComponentInChildren<Player>() != null)
        {
            buttonisShow = true;
        }
    }

    private void OnTriggerExit2D(Collider2D collision)
    {
        if (collision.GetComponentInChildren<Player>() != null)
        {
            buttonisShow = false;
        }
    }
}
