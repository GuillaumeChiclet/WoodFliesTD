using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class MapData
{
    public int width;
    public int height;
    public Array2D<Cell> cells;
    public WaypointGraph waypointGraph;
}

[RequireComponent(typeof(MapDisplay))]
public class Map : MonoBehaviour
{
    public int width = 100;
    public int height = 100;

    public ScriptableCell[] cellTypes;

    MeshRenderer meshRenderer;
    MeshFilter meshFilter;

    [HideInInspector] public MapData data;

    MapDisplay display;

    private void Awake()
    {
        display = GetComponent<MapDisplay>();
    }

    private void Start()
    {
        UpdateMesh();
    }

    public void UpdateMesh()
    {
        Material[] materials = new Material[cellTypes.Length];
        for (int i = 0; i < cellTypes.Length; i++)
            materials[i] = cellTypes[i].material;
        display.DrawMesh(MeshGenerator.CreateMeshFromCells(ref data.cells, cellTypes.Length, MapCoordinates.unitSize), materials);
    }

    public void GenerateEmpty()
    {
        ScriptableCell cellType = cellTypes[0];
        data = new MapData();
        data.cells = new Array2D<Cell>();
        data.cells.Initialize(width, height);
        data.width = width;
        data.height = height;
        data.waypointGraph = new WaypointGraph();
        data.waypointGraph.waypoints = new List<Waypoint>();

        for (int j = 0; j < height; j++)
        {
            for (int i = 0; i < width; i++)
            {
                SetCell(i, j, new Cell { id = 0, height = cellType.defaultHeight, isBuildable = cellType.isBuildable });
            }
            
        }
        Debug.Log(" ");
    }

    

    public Cell GetCellFromWorldPos(Vector3 position)
    {
        int x = 0, y = 0;
        MapCoordinates.WorldToCellCoords(position, ref x, ref y);
        return data.cells.Get(x, y);
    }

    public void SetCellFromID(int i, int j, int id, float height = -1)
    {
        float h = height >= 0.0f ? height : cellTypes[id].defaultHeight;
        SetCell(i, j, new Cell { id = id, height = h, isBuildable = cellTypes[id].isBuildable });
    }

    public void SetCell(int i, int j, Cell cell)
    {
        data.cells.Set(i, j, cell);
    }
}
