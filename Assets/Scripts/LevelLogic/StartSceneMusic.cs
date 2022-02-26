using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class StartSceneMusic : MonoBehaviour
{
    private static StartSceneMusic Music;
    // Start is called before the first frame update
    void Awake()
    {
        if(Music==null)
        {
            Music = this;
            DontDestroyOnLoad(Music);
        }
        else
        {
            Destroy(gameObject);
        }
    }
}
