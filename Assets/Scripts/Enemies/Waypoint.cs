using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class Waypoint
{
    public Vector2Int cellCoords = Vector2Int.zero;
    public List<int> nextIndices = new List<int>();
}
