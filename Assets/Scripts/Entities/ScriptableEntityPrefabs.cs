using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "EntityPrefabs", menuName = "Resources/ScriptableEntityPrefab")]
public class ScriptableEntityPrefabs : ScriptableObject
{
    public GameObject[] cellEntityPrefabs;
}
