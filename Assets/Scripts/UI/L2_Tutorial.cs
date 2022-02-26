using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class L2_Tutorial : MonoBehaviour
{
    private Animator anim;
    private SpriteRenderer sr;
    private bool buttonisShow;

    void Start()
    {
        anim = gameObject.GetComponent<Animator>();
        sr = gameObject.GetComponent<SpriteRenderer>();
        sr.enabled = false;
    }

    // Update is called once per frame
    void Update()
    {
        if (buttonisShow)
        {
            sr.enabled = true;
        }

        if (GetComponentsInChildren<Transform>(true).Length <= 8)
        {
            Destroy(gameObject);
        }
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        buttonisShow = true;
    }
}
