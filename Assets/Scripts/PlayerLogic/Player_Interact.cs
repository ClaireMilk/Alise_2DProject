using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;

public partial class Player : MonoBehaviour
{

   public void OpenTheDoor(GameObject door,float doorHeight,float duration)
    {
        Tween tween = door.transform.DOMove(new Vector3(door.transform.position.x, door.transform.position.y + doorHeight, door.transform.position.z), duration);
    }
}
