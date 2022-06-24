using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "ResourceHandler", menuName = "Map/Resource Handler", order = 0)]
public class ResourceHandler : ScriptableObject
{
    [SerializeField] List<ScriptableResource> resources;

    public void Initialize() 
    {
        for (int index = 0; index < resources.Count; index++)
            resources[index].ID = index;
    }

    public ScriptableResource IdToResource(int id) 
    {
        return resources[id];
    }
}

