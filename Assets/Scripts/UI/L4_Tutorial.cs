using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class L4_Tutorial : MonoBehaviour
{
    private Animator anim;
    private SpriteRenderer sr;
    private bool buttonisShow;
    public GameObject Tutorial;

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
            Tutorial.SetActive(false);
        }
        else
        {
            sr.enabled = true;
            Tutorial.SetActive(true);
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
