using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public  class TutorialLogic : MonoBehaviour
{
    //public string MovementMessage;
    //public string JumpMessage;
    public TMP_Text MovementInstruction;
    public TMP_Text JumpInstruction;
    public TMP_Text AttackInstruction;
    public SpriteRenderer ArrowUpSpriteRender;
    public SpriteRenderer ArrowForwardSpriteRender;


    // Start is called before the first frame update
    void Start()
    {
        MovementInstruction.GetComponent<Text>();
        MovementInstruction.enabled = true;
        JumpInstruction.enabled = false;
        AttackInstruction.enabled = false;
        
    }

    // Update is called once per frame
    void Update()
    {
      
    }
}
