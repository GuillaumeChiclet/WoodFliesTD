using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "AssetsList", menuName = "AssetsList")]
public class ScriptableAssets : ScriptableObject
{
    public GameObject[] resourcesPrefabs;
    public GameObject[] playerPrefabs;
    public GameObject[] buildingPrefabs;
}
