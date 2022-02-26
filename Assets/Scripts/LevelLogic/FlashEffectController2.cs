using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FlashEffectController2 : MonoBehaviour
{
    public GameObject Flash;
    public GameObject AnotherFlash;
    public float TimeUnitlFlashing;
    public SpriteRenderer flashsprite;
    public float TimeUnitilTurnOffSprite;
    // Start is called before the first frame update
    void Start()
    {
       
    }

    // Update is called once per frame
    void Update()
    {
        if (Input.GetMouseButtonDown(0))
        {
            TimeUnitlFlashing -= Time.time;
            TimeUnitilTurnOffSprite -= Time.time;
            if (TimeUnitlFlashing <= 0)
            {
                Instantiate(Flash, transform.position, transform.rotation);
                Destroy(AnotherFlash);
            }
            if (TimeUnitilTurnOffSprite <= 0)
            {
                {
                    flashsprite.enabled = false;
                    Destroy(Flash,0.1f);
                }
            }
        }
    }
    //public void ControlQuitFlash()
    //{
    //    if (Input.GetMouseButtonDown(0))
    //    {
    //        TimeUnitlFlashing -= Time.time;
    //        TimeUnitilTurnOffSprite -= Time.time;
    //        if (TimeUnitlFlashing <= 0)
    //        {
    //            Instantiate(Flash, transform.position, transform.rotation);
    //        }
    //        if (TimeUnitilTurnOffSprite <= 0)
    //        {
    //            {
    //                flashsprite.enabled = false;
    //            }
    //        }
    //    }
    //}
}
