using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Resources : MonoBehaviour
{
    
    public ResourceHandler resourceList;
    public static ResourceHandler handler;
    void Start() 
    {
        handler = resourceList;
        resourceList.Initialize();
    }
}
