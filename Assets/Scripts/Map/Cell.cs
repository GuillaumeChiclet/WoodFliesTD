using System.Collections;
using System.Collections.Generic;
using UnityEngine;


[System.Serializable]
public class Cell
{
    public int id = 0;
    public float height = 0;
    public bool isBuildable = false;

    public string ownedEntityName = "";
    public string ownedEntityData = "";
    [System.NonSerialized] public CellEntity ownedEntity = null;

    public void CleanCellEntity()
    {
        ownedEntity = null;
        ownedEntityName = "";
        ownedEntityData = "";
    }

    public bool TryGetCellEntity(out CellEntity cellEntity)
    {
        if (ownedEntity)
        {
            cellEntity = ownedEntity;
            return true;
        }
        cellEntity = null;
        return false;
    }

    public void Save()
    {
        if (ownedEntity)
        {
            ownedEntityData = ownedEntity ? ownedEntity.ToJson() : "";
        }
        else
        {
            CleanCellEntity();
        }
    }
}
