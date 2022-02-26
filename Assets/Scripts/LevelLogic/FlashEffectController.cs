using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FlashEffectController : MonoBehaviour
{
    public GameObject Flash;
    float FlashSize;
    //public float TimeUnitlFlashing;
    public SpriteRenderer flashsprite;
    public float TimeUnitilTurnOffSprite;
    Vector3 mouseClickPos;


    public GameObject CanvasGameObject;
    public GameObject CameraGameObject;
  

    // Start is called before the first frame update
    void Start()
    {
        flashsprite.enabled = true;
        Flash.transform.localScale = Vector3.one * FlashSize;
        
    }

    // Update is called once per frame
    void Update()
    {
        if (Input.GetMouseButtonDown(0))
        {
            mouseClickPos = Input.mousePosition;            
            DisplayFlash(TransformToScreenPos(mouseClickPos));
            Instantiate(Flash, TransformToScreenPos(mouseClickPos), transform.rotation);
            //TimeUnitilTurnOffSprite -= 1;                    
        }
        //if (Flash && FlashSize > 0)
        //{
            
        //    FlashDisappears(TimeUnitilTurnOffSprite);
        //}

    }    
    public void DisplayFlash(Vector3 pos)
    {
        FlashSize = 70.0f;
        Flash.transform.localScale = Vector3.one * FlashSize;
        Flash.transform.position = pos;
    }
    public Vector3 TransformToScreenPos(Vector3 position)
    {
        float posX = position.x * CanvasGameObject.transform.position.x / CameraGameObject.transform.position.x;
        float posY = position.y * CanvasGameObject.transform.position.y / CameraGameObject.transform.position.y;
        float posZ = position.z * CanvasGameObject.transform.position.z / CameraGameObject.transform.position.z;
        Vector3 transformedPos = new Vector3(posX, posY, posZ);
        return transformedPos;
    }
    //public void FlashDisappears(float TimeToDisappear)
    //{
    //    TimeToDisappear -= Time.deltaTime;
    //    FlashSize -= Time.deltaTime;
    //    Flash.transform.localScale = Vector3.one * 0;
    //}

}
