using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CellEntity : MonoBehaviour, ISerializable
{
    public Cell cell = null;
    public virtual string ToJson() { return ""; }
    public virtual void FromJson(string json) { }
}
