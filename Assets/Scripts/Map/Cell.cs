using System.Collections;
using System.Collections.Generic;
using UnityEngine;


[System.Serializable]
public class Cell
{
    public int id = 0;
    public float height = 0;
    public bool isBuildable = false;
    [System.NonSerialized] public CellEntity ownedEntity;
}
