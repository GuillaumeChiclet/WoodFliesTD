using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "New CellType", menuName = "Assets/Scriptable CellType", order = 0)]
public class ScriptableCellType : ScriptableObject
{
    public string typeName = "DefaultCell";
    public float defaultHeight = 0;
    public bool isBuildable = true;
    public Material material = null;
}
