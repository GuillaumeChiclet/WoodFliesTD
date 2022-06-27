using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AssetsManager : MonoBehaviour
{
    public ScriptableAssets scriptableAssets;

    public Dictionary<string, GameObject> resourceEntityPrefabs;
    public Dictionary<string, GameObject> buildingsEntityPrefabs;
    public Dictionary<string, GameObject> playerEntityPrefabs;

    public void Initialize()
    {
        resourceEntityPrefabs = new Dictionary<string, GameObject>();
        foreach(GameObject go in scriptableAssets.resourcesPrefabs)
        {
            resourceEntityPrefabs.Add(go.name, go);
        }

        buildingsEntityPrefabs = new Dictionary<string, GameObject>();
        foreach (GameObject go in scriptableAssets.buildingPrefabs)
        {
            buildingsEntityPrefabs.Add(go.name, go);
        }

        playerEntityPrefabs = new Dictionary<string, GameObject>();
        foreach (GameObject go in scriptableAssets.playerPrefabs)
        {
            playerEntityPrefabs.Add(go.name, go);
        }
    }

    public bool TryGet(string name, out GameObject go)
    {
        if (resourceEntityPrefabs.ContainsKey(name))
        {
            go = resourceEntityPrefabs[name];
            return true;
        }
        if (buildingsEntityPrefabs.ContainsKey(name))
        {
            go = buildingsEntityPrefabs[name];
            return true;
        }
        if (playerEntityPrefabs.ContainsKey(name))
        {
            go = playerEntityPrefabs[name];
            return true;
        }

        go = null;
        return false;
    }
}
