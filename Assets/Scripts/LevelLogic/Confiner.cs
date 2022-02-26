using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Cinemachine;

public class Confiner : MonoBehaviour
{
   GameObject mainCam;
    private void Awake()
    {
        mainCam = GameObject.FindGameObjectWithTag("CM");
    }
    private void OnTriggerEnter2D(Collider2D collision)
    {
        if(collision.gameObject.tag=="Player")
        {
            mainCam.GetComponent<CinemachineConfiner>().m_BoundingShape2D = this.GetComponent<PolygonCollider2D>();
        }
    }
}
