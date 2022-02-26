using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Tutorial_SecondPlatformLogic : TutorialLogic
{
    // Start is called before the first frame update
    void Start()
    {
        ArrowForwardSpriteRender.enabled = false;
    }

    // Update is called once per frame
    void Update()
    {
        
    }
    private void OnTriggerEnter2D(Collider2D other)
    {
        if(other.tag=="Player")
        {
            ArrowForwardSpriteRender.enabled = true;
        }
    }
    private void OnTriggerExit2D(Collider2D other)
    {
        if(other.tag=="Player")
        {
            ArrowForwardSpriteRender.enabled = false;
        }
    }
}
