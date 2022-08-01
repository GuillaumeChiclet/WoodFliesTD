using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.InputSystem;
using TMPro;


public enum EditMode
{
    None,
    Cell,
    Resource,
    Building,
    Waypoints,
    WaypointsLink,
    WaypointsStart,
    WaypointsEnd
}


public class MapEditor : MonoBehaviour
{
    public Camera cam;
    public LayerMask terrainLayerMask;

    [Header("UI")]
    public GameObject editPanel;

    [Header("Edit Toggles")]
    public Toggle cellToggle;
    public Toggle resourceToggle;
    public Toggle buildingToggle;
    public Toggle waypointToggle;
    public Toggle waypointLinkToggle;
    public Toggle waypointStartToggle;
    public Toggle waypointEndToggle;

    [Header("Resources")]
    public TMP_Dropdown resource_dropdown;

    [Header("Cells")]
    public TMP_InputField cellHeight_text;
    public Slider cellHeight_slider;
    public TMP_Dropdown type_dropdown;

    [Header("Camera")]
    //public TMP_Text cameraZoom_text;
    public TMP_InputField cameraZoom_text;
    public Slider cameraZoom_slider;
    public TMP_InputField cameraHeight_text;
    public Slider cameraHeight_slider;

    [Header("Map")]
    public TMP_InputField mapWidth_inputField;
    public TMP_InputField mapHeight_inputField;


    [Header("Load&Save")]
    public TMP_InputField saveName_inputField;

    [Header("Visuals Utils")]
    public GameObject spherePrefab = null;
    public Material defaultMaterial = null;


    // Current Edit settings
    EditMode currentEditMode = EditMode.None;
    float currentCellHeight = 0.0f;
    float cameraZoom = 10.0f;
    float cameraHeight = 10.0f;

    // References to the map and managers
    Map map = null;
    AssetsManager assetsManager = null;
    SaveLoadManager saveLoadManager = null;

    bool isMouseDown = false;

    Vector2Int firstClic;
    Vector2Int secondClic;

    List<GameObject> waypointGO;

    void Start()
    {
        GameObject goa = GameObject.Find("AssetsManager");
        if (goa) assetsManager = goa.GetComponent<AssetsManager>();

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

        waypointGO = new List<GameObject>();

        resource_dropdown.ClearOptions();
        string[] tmp = new string[assetsManager.resourceEntityPrefabs.Count];
        assetsManager.resourceEntityPrefabs.Keys.CopyTo(tmp, 0);
        List<string> rNames = new List<string>(tmp);
        resource_dropdown.AddOptions(rNames);

        currentCellHeight = map.cellTypes[type_dropdown.value].defaultHeight;
        cellHeight_text.text = (Get2Decimal(currentCellHeight).ToString());
        cellHeight_slider.value = Get2Decimal(currentCellHeight);

        cameraZoom = cam.orthographicSize;
        cameraHeight = cam.transform.position.y;
        cameraZoom_text.text = Get2Decimal(cameraZoom).ToString();
        cameraHeight_text.text = Get2Decimal(cameraHeight).ToString();
        cameraZoom_slider.value = Get2Decimal(cameraZoom);
        cameraHeight_slider.value = Get2Decimal(cameraHeight);

        if (PlayerPrefs.HasKey("sceneName"))
        {
            saveName_inputField.text = PlayerPrefs.GetString("sceneName");
        }

        UpdateWaypointsGO();
    }

    // Update is called once per frame
    void Update()
    {
        if (isMouseDown && GetHoveredCellPosition(out Vector2Int cellCoord))
        {
            if (cellToggle.isOn)
            {
                map.SetCellFromID(cellCoord.x, cellCoord.y, type_dropdown.value, false, currentCellHeight);
                map.UpdateMesh();
            }
        }
    }


    public void CheckEditMode(bool value)
    {
        if (!value)
        {
            currentEditMode = EditMode.None;
            return;
        }

        switch (currentEditMode)
        {
            case EditMode.Cell:
                cellToggle.isOn = false;
                break;
            case EditMode.Building:

                break;
            case EditMode.Resource:
                resourceToggle.isOn = false;
                break;
            case EditMode.Waypoints:
                waypointToggle.isOn = false;
                break;
            case EditMode.WaypointsLink:
                waypointLinkToggle.isOn = false;
                break;
            case EditMode.WaypointsStart:
                waypointStartToggle.isOn = false;
                break;
            case EditMode.WaypointsEnd:
                waypointEndToggle.isOn = false;
                break;
            case EditMode.None:

                break;
            default:
                break;
        }

        if (cellToggle.isOn) currentEditMode = EditMode.Cell;
        else if (resourceToggle.isOn) currentEditMode = EditMode.Resource;
        else if (waypointToggle.isOn) currentEditMode = EditMode.Waypoints;
        else if (waypointLinkToggle.isOn) currentEditMode = EditMode.WaypointsLink;
        else if (waypointStartToggle.isOn) currentEditMode = EditMode.WaypointsStart;
        else if (waypointEndToggle.isOn) currentEditMode = EditMode.WaypointsEnd;
        else currentEditMode = EditMode.None;
    }

    /*
    public void UpdateEditMode(EditMode editMode)
    {
        cellToggle.isOn = editMode == EditMode.Cell;
        resourceToggle.isOn = editMode == EditMode.Resource;
        waypointToggle.isOn = editMode == EditMode.Waypoints;
        waypointLinkToggle.isOn = editMode == EditMode.WaypointsLink;
        waypointStartToggle.isOn = editMode == EditMode.WaypointsStart;
        waypointEndToggle.isOn = editMode == EditMode.WaypointsEnd;
    }*/

    public void Resize()
    {
        if (int.TryParse(mapWidth_inputField.text, out int width) && int.TryParse(mapHeight_inputField.text, out int height))
        {
            Debug.Log("Resizing with " + width.ToString() + " " + height.ToString());
            map.Initialize(width, height);
            map.UpdateMesh();
        }
    }

    public void SetCellType(int value)
    {
        SetCellHeight(map.cellTypes[value].defaultHeight);
    }

    public void SetCellHeight(string value)
    {
        if (value == "") return;
        if (value[value.Length - 1] == ',') return;
        if (float.TryParse(value, out float height))
        {
            currentCellHeight = Get2Decimal(GetStep(height));
            cellHeight_slider.SetValueWithoutNotify(currentCellHeight);
        }
    }
    public void SetCellHeight(float height)
    {
        currentCellHeight = Get2Decimal(GetStep(height));
        cellHeight_slider.SetValueWithoutNotify(currentCellHeight);
        cellHeight_text.SetTextWithoutNotify(currentCellHeight.ToString());
    }

    public void SetCameraZoom(string value)
    {
        
        if (value == "") return;
        if (value[value.Length - 1] == '.') return;
        if (float.TryParse(value.Replace(',', '.'), out float result))
            SetCameraZoom(result);
    }
    public void SetCameraZoom(float zoom)
    {
        cameraZoom = Get2Decimal(GetStep(zoom));
        cameraZoom_text.text = cameraZoom.ToString();
        cameraZoom_slider.value = cameraZoom;
        cam.orthographicSize = zoom;
    }

    public void SetCameraHeight(string value)
    {
        if (value == "") return;
        if (value[value.Length - 1] == '.') return;
        if (System.Single.TryParse(value.Replace(',', '.'), out float result))
            SetCameraHeight(result);
    }
    public void SetCameraHeight(float height)
    {
        cameraHeight = Get2Decimal(GetStep(height));
        cameraHeight_text.text = cameraHeight.ToString();
        cameraHeight_slider.value = cameraHeight;
        Vector3 pos = cam.transform.position;
        pos.y = cameraHeight;
        cam.transform.position = pos;
    }


    public void SaveMap()
    {
        saveLoadManager.Save(saveName_inputField.text);
    }

    public void LoadMap()
    {
        map.ClearToDefault();
        saveLoadManager.Load(saveName_inputField.text);
        map.UpdateMesh();
    }
    public void ResetMap()
    {
        map.ClearToDefault();
        map.UpdateMesh();
    }


    void UpdateWaypointsGO()
    {
        if (!map)
            return;

        for (int i = 0; i < waypointGO.Count; i++)
        {
            Destroy(waypointGO[i]);
        }
        waypointGO.Clear();

        for (int i = 0; i < map.data.waypointGraph.waypoints.Count; i++)
        {
            Waypoint wp = map.data.waypointGraph.waypoints[i];
            map.data.cells.TryGet(wp.cellCoords.x, wp.cellCoords.y, out Cell cell);
            GameObject go = Instantiate(spherePrefab, MapCoordinates.CellToWorldCoords(wp.cellCoords, cell.height), Quaternion.identity, transform);
            waypointGO.Add(go);

            for (int j = 0; j < wp.nextIndices.Count; j++)
            {
                Vector2Int coords = map.data.waypointGraph.waypoints[wp.nextIndices[j]].cellCoords;
                LineRenderer line = go.AddComponent<LineRenderer>();
                line.startWidth = .5f;
                line.endWidth = .5f;
                line.material = defaultMaterial;
                Vector3[] points = new Vector3[2] { MapCoordinates.CellToWorldCoords(wp.cellCoords, cell.height), MapCoordinates.CellToWorldCoords(coords, cell.height) };
                line.SetPositions(points);
            }
        }

    }

    /*
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
    }*/

    public void OnActivate(InputAction.CallbackContext context) => editPanel.SetActive(!editPanel.activeSelf);

    public void OnMouseLeftButton(InputAction.CallbackContext context)
    {
        if (editPanel.activeSelf)
            return;

        bool isPressed = context.action.IsPressed();

        if (isPressed)
        {
            OnMouseLeftButtonPressed();
        }
        else
        {
            OnMouseLeftButtonReleased();
        }
    }


    void OnMouseLeftButtonPressed()
    {
        if (isMouseDown)
            return;

        isMouseDown = true;

        if (GetHoveredCellPosition(out Vector2Int cellCoord))
        {
            if (waypointToggle.isOn)
            {
                if (map.data.waypointGraph.HasWaypoint(cellCoord))
                    map.data.waypointGraph.RemoveWaypoint(cellCoord);
                else
                    map.data.waypointGraph.AddWaypoint(cellCoord);

                UpdateWaypointsGO();
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

            if (resourceToggle.isOn)
            {
                if (!map.data.cells.TryGet(cellCoord, out Cell cell))
                    return;

                if (cell.TryGetCellEntity(out CellEntity cellEntity))
                    map.TryDestroyCellEntity(cellCoord);
                else
                    map.TrySpawnCellEntity(cellCoord, resource_dropdown.captionText.text, out CellEntity entity);
            }
        }
    }

    void OnMouseLeftButtonReleased()
    {
        if (!isMouseDown)
            return;

        isMouseDown = false;

        if (GetHoveredCellPosition(out Vector2Int cellCoord))
        {
            if (waypointLinkToggle.isOn)
            {
                secondClic = cellCoord;
                map.data.waypointGraph.LinkOrUnlinkWaypoints(firstClic, secondClic);

                UpdateWaypointsGO();
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

    private float Get2Decimal(float value)
    {
        return (float)((int)(value * 100.0f)) / 100.0f;
    }

    private float GetStep(float value, float step = 0.25f)
    {
        return value - value % 0.25f;
    }
}
