using System.Collections;
using System.Collections.Generic;
using UnityEngine;


[System.Serializable]
public class MapSaveData
{
    public int width;
    public int height;
    public List<Cell> cells;
    public WaypointGraph waypointGraph;
}

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
    AssetsManager assets;

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

    public void Initialize()
    {
        GameObject go = GameObject.Find("AssetsManager");
        if (go)
            assets = go.GetComponent<AssetsManager>();

        data = new MapData();

        data.width = width;
        data.height = height;

        data.cells = new Array2D<Cell>();
        data.cells.Initialize(width, height);
        
        data.waypointGraph = new WaypointGraph();
        data.waypointGraph.waypoints = new List<Waypoint>();

        ClearToDefault();
    }

    public void ClearToDefault()
    {
        ScriptableCell cellType = cellTypes[0];
        for (int j = 0; j < height; j++)
        {
            for (int i = 0; i < width; i++)
            {
                TryDestroyCellEntity(i, j);
                data.cells.TrySet(i, j, new Cell { id = 0, height = cellType.defaultHeight, isBuildable = cellType.isBuildable });
            }
        }
    }

    public bool TryGetCellEntity(Vector2Int coords, ref CellEntity cellEntity)
    {
        if (data.cells.TryGet(coords, out Cell cell))
        {
            return cell.TryGetCellEntity(out cellEntity);
        }
        return false;
    }

    public bool TryGetCellFromWorldPos(Vector3 position, out Cell cell)
    {
        int x = 0, y = 0;
        MapCoordinates.WorldToCellCoords(position, ref x, ref y);
        return data.cells.TryGet(x, y, out cell);
    }

    public void SetCellFromID(int i, int j, int id, float height = -1)
    {
        float h = height >= 0.0f ? height : cellTypes[id].defaultHeight;
        if (data.cells.TryGet(i, j, out Cell cell))
        {
            cell.height = h;
            cell.id = id;
            cell.isBuildable = cellTypes[id].isBuildable;
        }
    }

    public bool TrySpawnCellEntity(Vector2Int coords, string entityName, out CellEntity cellEntity)
    {
        return TrySpawnCellEntity(coords.x, coords.y, entityName, out cellEntity);
    }
    public bool TrySpawnCellEntity(int i, int j, string entityName, out CellEntity cellEntity)
    {
        if (data.cells.TryGet(i, j, out Cell cell))
        {
            if (cell.ownedEntity == null) // If no entity on the cell
            {
                if (assets.TryGet(entityName, out GameObject go)) // If we found appropriate Prefab
                {
                    GameObject prefab = Instantiate(go, MapCoordinates.CellToWorldCoords(i, j, cell.height), Quaternion.identity, transform);
                    cellEntity = prefab.GetComponent<CellEntity>();

                    cellEntity.cell = cell;
                    cell.ownedEntity = cellEntity;
                    cell.ownedEntityName = entityName;
                    return true;
                }
            }
        }
        cellEntity = null;
        return false;
    }

    public bool TryDestroyCellEntity(Vector2Int coords)
    {
        return TryDestroyCellEntity(coords.x, coords.y);
    }

    public bool TryDestroyCellEntity(int i, int j)
    {
        if (data.cells.TryGet(i, j, out Cell cell))
        {
            if (cell == null)
                return false;

            CellEntity entity = cell.ownedEntity;
            if (entity)
            {
                cell.CleanCellEntity();
                Destroy(entity.gameObject);
                return true;
            }
        }
        return false;
    }


    public void LoadMapSaveData(MapSaveData mapSaveData)
    {
        data.width = mapSaveData.width;
        data.height = mapSaveData.height;
        data.cells.CreateFromList(mapSaveData.cells, mapSaveData.width, mapSaveData.height);
        data.waypointGraph = mapSaveData.waypointGraph;

        for (int i = 0; i < data.width; i++)
        {
            for (int j = 0; j < data.height; j++)
            {
                if (data.cells.TryGet(i, j, out Cell cell))
                {
                    if (cell.ownedEntityName != "")
                    {
                        if (assets.TryGet(cell.ownedEntityName, out GameObject go))
                        {
                            GameObject prefab = Instantiate(go, MapCoordinates.CellToWorldCoords(i, j, cell.height), Quaternion.identity, transform);
                            CellEntity entity = prefab.GetComponent<CellEntity>();
                            entity.FromJson(cell.ownedEntityData);
                            entity.cell = cell;
                            cell.ownedEntity = entity;
                        }
                    }
                }
            }
        }
    }

    public MapSaveData GetMapSaveData()
    {
        for (int i = 0; i < data.width; i++)
        {
            for (int j = 0; j < data.height; j++)
            {
                if (data.cells.TryGet(i, j, out Cell cell))
                {
                    cell.Save();
                }
            }
        }

        return new MapSaveData
        {
            width = data.width,
            height = data.height,
            cells = data.cells.GetAsList(),
            waypointGraph = data.waypointGraph,
        };
        
    }
}
