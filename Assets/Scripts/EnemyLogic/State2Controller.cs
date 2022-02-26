using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Experimental.U2D.Animation;

public class State2Controller : MonoBehaviour
{
    public List<SpriteResolver> spriteResolvers = new List<SpriteResolver>();
    private void Start()
    {
        foreach(var resolver in FindObjectsOfType<SpriteResolver>())
        {
            spriteResolvers.Add(resolver);
         //   resolver.SetCategoryAndLabel(resolver.GetCategory(), "Normal");
        }
        foreach (var resolver in FindObjectsOfType<SpriteResolver>())
        {
            resolver.SetCategoryAndLabel(resolver.GetCategory(), "Normal");
        }
    }
    private void Update()
    {
        if(Input.GetKeyDown(KeyCode.E))
        {
            foreach (var resolver in FindObjectsOfType<SpriteResolver>())
            {
                resolver.SetCategoryAndLabel(resolver.GetCategory(), "State2");
            }
        }
    }
}
