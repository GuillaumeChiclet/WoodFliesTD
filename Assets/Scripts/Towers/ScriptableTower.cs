using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "New Tower", menuName = "Assets/Scriptable Tower", order = 0)]
public class ScriptableTower : ScriptableObject
{
    public List<ScriptableResource> resourceList;
    public List<int>                resourceListCost;

    public int baseDamage = 10; // hit points
    public int range = 5; // meters
    public int attackSpeed = 1; // seconds
}

