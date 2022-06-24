using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public static class MapCoordinates
{
    public static float unitSize = 1.0f;

    public static void WorldToCellCoords(Vector3 position, ref int x, ref int y)
    {
        x = (int)(position.x / unitSize + unitSize * 0.5f);
        y = (int)(position.z / unitSize + unitSize * 0.5f);
    }
    public static Vector2Int WorldToCellCoords(Vector3 position)
    {
        return new Vector2Int((int)(position.x / unitSize + unitSize * 0.5f), (int)(position.z / unitSize + unitSize * 0.5f));
    }

    public static Vector3 CellToWorldCoords(int x, int y, float height = 0)
    {
        return new Vector3(x * unitSize, height, y * unitSize);
    }

    public static Vector3 CellToWorldCoords(Vector2Int coords, float height = 0)
    {
        return new Vector3(coords.x * unitSize, height, coords.y * unitSize);
    }
}
