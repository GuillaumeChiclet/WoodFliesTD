using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class CellEntity : MonoBehaviour, ISerializable, IInteractable
{
    public Cell cell = null;
    public virtual string ToJson() { return ""; }
    public virtual void FromJson(string json) { }

    public virtual void PrimarAction(GameObject caller) { }
    public virtual void SecondAction(GameObject caller) { }
}
