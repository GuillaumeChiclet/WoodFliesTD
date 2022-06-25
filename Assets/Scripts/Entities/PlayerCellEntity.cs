using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerCellEntity : CellEntity, IInteractable
{
    public void PrimarAction(GameObject caller)
    {
        Debug.Log("Interacting with turret " + gameObject.name);
    }

    public void SecondAction(GameObject caller)
    {
        throw new System.NotImplementedException();
    }

    // Start is called before the first frame update
    void Start()
    {

    }

    // Update is called once per frame
    void Update()
    {

    }
}
