
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

[CreateAssetMenu(fileName = "Resource", menuName = "Map/Scriptable Resource", order = 0)]
public class ScriptableResource : ScriptableObject
{
    public string displayName   = "DefaultResource";
    public Sprite sprite;

    [HideInInspector] public int ID = -1;
}