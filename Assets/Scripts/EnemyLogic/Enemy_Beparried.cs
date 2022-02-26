using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Enemy_Beparried : MonoBehaviour
{
    public int TimesOfPerfectParries=0;
    // Start is called before the first frame update
    

    // Update is called once per frame
    
    public void OntriggerEnter2D(Collider2D collision)
    {
        if(collision.tag=="ParryCollider")
        {
            TimesOfPerfectParries += 1;
            //m_SpriteRenderer.color = Color.yellow;
        }
        if(TimesOfPerfectParries==3)
        {
            ParriedStiff();
            //m_SpriteRenderer.color = Color.magenta;
        }
    }
    public void ParriedStiff()
    {
        //EnemyAnim.SetTrigger("")  Require further production of a necessary animation
    }
    
}
