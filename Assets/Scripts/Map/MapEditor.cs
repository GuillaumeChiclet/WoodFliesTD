using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
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
    public TMP_Text height_text;
    public TMP_Text zoom_text;
    public TMP_InputField saveName_inputField;
    public TMP_Dropdown type_dropdown;



    float currentCellHeight = 0.0f;
    float currentZoom = 10.0f;

    Map map;
    SaveLoadManager saveLoadManager;
    bool panelOpen = true;

    Vector2Int firstClic;
    Vector2Int secondClic;


    void Awake()
    {
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
        currentCellHeight = map.cellTypes[type_dropdown.value].defaultHeight;
        height_text.text = (currentCellHeight.ToString());
    }

    // Update is called once per frame
    void Update()
    {
        // Open or Close EditorPanel
        if (Input.GetKeyDown(KeyCode.Tab))
        {
            panelOpen = !panelOpen;
            editPanel.SetActive(panelOpen);
        }

        if (panelOpen)
            return;


        Ray ray = cam.ScreenPointToRay(Input.mousePosition);
        bool touch = Physics.Raycast(ray, out RaycastHit hit, 5000, terrainLayerMask, QueryTriggerInteraction.Ignore);

        int x = 0, y = 0;
        if (touch && hit.collider.gameObject.layer != LayerMask.NameToLayer("UI"))
        {
            MapCoordinates.WorldToCellCoords(hit.point - ray.direction * 0.05f, ref x, ref y);
        }


        // Raycast to EDIT
        if (editToggle.isOn && Input.GetMouseButton(0))
        {
            map.SetCellFromID(x, y, type_dropdown.value, currentCellHeight);
            map.UpdateMesh();
        }

        if (waypointToggle.isOn && Input.GetMouseButtonDown(0))
        {
            Debug.Log("X: " + x.ToString() + " - Y: " + y.ToString());
            Vector2Int coords = new Vector2Int(x, y);
            if (map.data.waypointGraph.HasWaypoint(coords))
                map.data.waypointGraph.RemoveWaypoint(coords);
            else
                map.data.waypointGraph.AddWaypoint(coords);
        }
        
        if (waypointLinkToggle.isOn)
        {
            if (Input.GetMouseButtonDown(0))
            {
                Debug.Log("X: " + x.ToString() + " - Y: " + y.ToString());
                firstClic = new Vector2Int(x, y);
            }
            else if (Input.GetMouseButtonUp(0))
            {
                Debug.Log("X: " + x.ToString() + " - Y: " + y.ToString());
                secondClic = new Vector2Int(x, y);
                map.data.waypointGraph.LinkOrUnlinkWaypoints(firstClic, secondClic);
            }
        }

        if (waypointStartToggle.isOn && Input.GetMouseButtonDown(0))
        {
            Waypoint wp = null;
            int idx = 0;
            if (map.data.waypointGraph.GetWaypoint(new Vector2Int(x, y), ref wp, ref idx))
            {
                map.data.waypointGraph.start = idx;
            }
        }

        if (waypointEndToggle.isOn && Input.GetMouseButtonDown(0))
        {
            Waypoint wp = null;
            int idx = 0;
            if (map.data.waypointGraph.GetWaypoint(new Vector2Int(x, y), ref wp, ref idx))
            {
                map.data.waypointGraph.end = idx;
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
        map.GenerateEmpty();
        map.UpdateMesh();
    }

    private void OnDrawGizmos()
    {
        if (!map)
            return;

        for (int i = 0; i < map.data.waypointGraph.waypoints.Count; i++)
        {
            Waypoint wp = map.data.waypointGraph.waypoints[i];
            Cell cell = map.data.cells.Get(wp.cellCoords.x, wp.cellCoords.y);
            Gizmos.color = Color.blue;
            Gizmos.DrawSphere(MapCoordinates.CellToWorldCoords(wp.cellCoords, cell.height), 0.1f);
            for (int j = 0; j < wp.nextIndices.Count; j++)
            {
                Vector2Int coords = map.data.waypointGraph.waypoints[wp.nextIndices[j]].cellCoords;
                Gizmos.DrawLine(MapCoordinates.CellToWorldCoords(wp.cellCoords, cell.height), MapCoordinates.CellToWorldCoords(coords, cell.height));
            }
        }
    }

}
