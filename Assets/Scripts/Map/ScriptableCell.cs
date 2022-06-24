using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "CellType", menuName = "Map/Scriptable Cell", order = 0)]
public class ScriptableCell : ScriptableObject
{
    public string typeName = "DefaultCell";
    public float defaultHeight = 0;
    public bool isBuildable = true;
    public Material material = null;
}
