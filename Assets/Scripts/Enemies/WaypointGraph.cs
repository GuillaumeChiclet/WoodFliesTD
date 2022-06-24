using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class WaypointGraph
{
    public List<Waypoint> waypoints;
    public int start = 0;
    public int end = 0;

    public bool HasWaypoint(Vector2Int coords)
    {
        for (int i = 0; i < waypoints.Count; i++)
        {
            if (waypoints[i].cellCoords == coords)
                return true;
        }
        return false;
    }

    public bool GetWaypoint(Vector2Int coords, ref Waypoint waypoint, ref int index)
    {
        for (int i = 0; i < waypoints.Count; i++)
        {
            if (waypoints[i].cellCoords == coords)
            {
                waypoint = waypoints[i];
                index = i;
                return true;
            }
        }
        return false;
    }

    public void AddWaypoint(Vector2Int coords)
    {
        waypoints.Add(new Waypoint { cellCoords = coords, nextIndices = new List<int>() });
    }
    
    public void RemoveWaypoint(Vector2Int coords)
    {
        int idx = 0;
        Waypoint waypoint = null;
        if (GetWaypoint(coords, ref waypoint, ref idx))
        {
            waypoints.RemoveAt(idx);
        }
    }

    public void LinkOrUnlinkWaypoints(Vector2Int coordsA, Vector2Int coordsB)
    {
        if (coordsA == coordsB)
            return;

        Waypoint A = null;
        Waypoint B = null;
        int idxA = 0;
        int idxB = 0;
        bool hasA = GetWaypoint(coordsA, ref A, ref idxA);
        bool hasB = GetWaypoint(coordsB, ref B, ref idxB);

        if (hasA && hasB)
        {
            for (int i = 0; i < A.nextIndices.Count; i++)
            {
                if (A.nextIndices[i] == idxB)
                {
                    A.nextIndices.RemoveAt(i);
                    return;
                }
            }

            for (int i = 0; i < B.nextIndices.Count; i++)
            {
                if (B.nextIndices[i] == idxA)
                {
                    B.nextIndices.RemoveAt(i);
                    return;
                }
            }
            A.nextIndices.Add(idxB);
        }
    }
}
