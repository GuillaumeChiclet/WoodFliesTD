using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.InputSystem;
using TMPro;


public class MapEditor : MonoBehaviour
{
    public Camera cam;
    public LayerMask terrainLayerMask;

    [Header("UI")]
    public GameObject editPanel;
    public Toggle editToggle;
    public Toggle waypointToggle;
    public Toggle waypointLinkToggle;
    public Toggle waypointStartToggle;
    public Toggle waypointEndToggle;
    public Toggle resourceEditToggle;
    public TMP_Dropdown resource_dropdown;
    public TMP_Text height_text;
    public TMP_Text zoom_text;
    public TMP_InputField saveName_inputField;
    public TMP_Dropdown type_dropdown;

    float currentCellHeight = 0.0f;
    float currentZoom = 10.0f;

    Map map;
    AssetsManager assets;
    SaveLoadManager saveLoadManager;

    bool isMouseDown = false;

    Vector2Int firstClic;
    Vector2Int secondClic;


    void Start()
    {
        GameObject goa = GameObject.Find("AssetsManager");
        if (goa) assets = goa.GetComponent<AssetsManager>();

        GameObject gos = GameObject.Find("SaveManager");
        if (gos) saveLoadManager = gos.GetComponent<SaveLoadManager>();


        GameObject go = GameObject.Find("MapManager");
        if (go) map = go.GetComponent<Map>();

        type_dropdown.ClearOptions();
        List<string> names = new List<string>();
        for (int i = 0; i < map.cellTypes.Length; i++)
        {
            names.Add(map.cellTypes[i].typeName);
        }
        type_dropdown.AddOptions(names);


        resource_dropdown.ClearOptions();
        string[] tmp = new string[assets.resourceEntityPrefabs.Count];
        assets.resourceEntityPrefabs.Keys.CopyTo(tmp, 0);
        List<string> rNames = new List<string>(tmp);
        resource_dropdown.AddOptions(rNames);

        currentCellHeight = map.cellTypes[type_dropdown.value].defaultHeight;
        height_text.text = (currentCellHeight.ToString());
    }

    // Update is called once per frame
    void Update()
    {
        if(isMouseDown && GetHoveredCellPosition(out Vector2Int cellCoord))
        {
            if (editToggle.isOn)
            {
                map.SetCellFromID(cellCoord.x, cellCoord.y, type_dropdown.value, currentCellHeight);
                map.UpdateMesh();
            }
        }
    }

    public void SetCurrentType(int value)
    {
        SetCurrentHeight(map.cellTypes[value].defaultHeight);
    }

    public void SetCurrentHeight(float height)
    {
        currentCellHeight = height;
        height_text.text = (currentCellHeight.ToString());
    }

    public void ModifyCurrentHeight(float add)
    {
        SetCurrentHeight(currentCellHeight + add);
    }

    public void SetCurrentZoom(float zoom)
    {
        currentZoom = zoom;
        zoom_text.text = (currentZoom.ToString());
        cam.orthographicSize = zoom;
    }

    public void ModifyCurrentZoom(float add)
    {
        SetCurrentZoom(currentZoom + add);
    }

    public void SaveMap()
    {
        saveLoadManager.Save(saveName_inputField.text);
    }

    public void LoadMap()
    {
        saveLoadManager.Load(saveName_inputField.text);
        map.UpdateMesh();
    }
    public void ResetMap()
    {
        map.ClearToDefault();
        map.UpdateMesh();
    }

    private void OnDrawGizmos()
    {
        if (!map)
            return;

        for (int i = 0; i < map.data.waypointGraph.waypoints.Count; i++)
        {
            Waypoint wp = map.data.waypointGraph.waypoints[i];
            map.data.cells.TryGet(wp.cellCoords.x, wp.cellCoords.y, out Cell cell);
            Gizmos.color = Color.blue;
            Gizmos.DrawSphere(MapCoordinates.CellToWorldCoords(wp.cellCoords, cell.height), 0.1f);
            for (int j = 0; j < wp.nextIndices.Count; j++)
            {
                Vector2Int coords = map.data.waypointGraph.waypoints[wp.nextIndices[j]].cellCoords;
                Gizmos.DrawLine(MapCoordinates.CellToWorldCoords(wp.cellCoords, cell.height), MapCoordinates.CellToWorldCoords(coords, cell.height));
            }
        }
    }
    public void OnActivate(InputAction.CallbackContext context) => editPanel.SetActive(!editPanel.activeSelf);

    public void OnMouseLeftButton(InputAction.CallbackContext context)
    {
        if (GetHoveredCellPosition(out Vector2Int cellCoord))
        {
            if (context.ReadValueAsButton())
            {
                isMouseDown = true;

                if (waypointToggle.isOn)
                {
                    if (map.data.waypointGraph.HasWaypoint(cellCoord))
                        map.data.waypointGraph.RemoveWaypoint(cellCoord);
                    else
                        map.data.waypointGraph.AddWaypoint(cellCoord);
                }

                if (waypointLinkToggle.isOn)
                {
                    firstClic = cellCoord;
                }

                if (waypointStartToggle.isOn)
                {
                    Waypoint wp = null;
                    int idx = 0;
                    if (map.data.waypointGraph.GetWaypoint(cellCoord, ref wp, ref idx))
                    {
                        map.data.waypointGraph.start = idx;
                    }
                }

                if (waypointEndToggle.isOn)
                {
                    Waypoint wp = null;
                    int idx = 0;
                    if (map.data.waypointGraph.GetWaypoint(cellCoord, ref wp, ref idx))
                    {
                        map.data.waypointGraph.end = idx;
                    }
                }

                if (resourceEditToggle.isOn)
                {
                    if (!map.data.cells.TryGet(cellCoord.x, cellCoord.y, out Cell cell))
                        return;

                    map.TrySpawnCellEntity(cellCoord.x, cellCoord.y, resource_dropdown.captionText.text, out CellEntity entity);
                }
            }
            else
            {
                isMouseDown = false;

                if (waypointLinkToggle.isOn)
                {
                    secondClic = cellCoord;
                    map.data.waypointGraph.LinkOrUnlinkWaypoints(firstClic, secondClic);
                }
            }
        }
    }

    private bool GetHoveredCellPosition(out Vector2Int cellCoord)
    {
        cellCoord = new Vector2Int(-1, -1);
        Ray ray = cam.ScreenPointToRay(Mouse.current.position.ReadValue());
        bool touch = Physics.Raycast(ray, out RaycastHit hit, 5000, terrainLayerMask, QueryTriggerInteraction.Ignore);

        int x = 0, y = 0;
        if (touch && hit.collider.gameObject.layer != LayerMask.NameToLayer("UI"))
        {
            MapCoordinates.WorldToCellCoords(hit.point - ray.direction * 0.05f, ref x, ref y);

            cellCoord.x = x;
            cellCoord.y = y;

            return true;
        }
        else
        {
            return false;
        }
    }
}
