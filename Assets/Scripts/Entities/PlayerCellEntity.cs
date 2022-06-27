using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerCellEntity : CellEntity
{
    public override void PrimarAction(GameObject caller)
    {
        Debug.Log("Interaction with turret");
    }

    public override void SecondAction(GameObject caller)
    {
        throw new System.NotImplementedException();
    }
}
