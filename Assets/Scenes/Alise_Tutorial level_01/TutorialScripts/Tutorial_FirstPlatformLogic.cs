using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Tutorial_FirstPlatformLogic : TutorialLogic
{
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }
    private void OnTriggerEnter2D(Collider2D collision)
    {
        if(collision.gameObject.tag=="Player")
        {
            JumpInstruction.enabled = false;
            ArrowUpSpriteRender.enabled = false;
        }
    }
}
