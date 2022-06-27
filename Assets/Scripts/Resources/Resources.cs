using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Resources : MonoBehaviour
{
    
    public ResourceHandler resourceList;
    public static ResourceHandler handler;

    void Awake() 
    {
        handler = resourceList;
        resourceList.Initialize();
    }
}
