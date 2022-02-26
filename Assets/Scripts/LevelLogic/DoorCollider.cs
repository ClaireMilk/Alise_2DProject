using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DoorCollider : MonoBehaviour
{
    public float doorHeight;
    public float duration;
    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.gameObject.tag == "Player")
        {
            collision.GetComponentInChildren<Player>().OpenTheDoor(this.gameObject.transform.parent.gameObject, doorHeight, duration);
        }
    }
    //private void OnTriggerExit2D(Collider2D collision)
    //{
    //    if(collision.gameObject.tag=="Player")
    //    {
    //        collision.GetComponentInChildren<Player>().OpenTheDoor(this.gameObject.transform.parent.gameObject, -4, duration);
    //    }
    //}
}
