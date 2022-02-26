using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PropAppear : MonoBehaviour
{
    public GameObject prop;
    private void Start()
    {
        Debug.Log("start");
        if (prop)
        {
            prop.SetActive(false);
        }

    }
    private void OnDestroy()
    {
        if(prop)
        prop.SetActive(true);
    }
}
