using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.IO;

[System.Serializable]
public class MapSaveData
{
    public int width;
    public int height;
    public List<Cell> cells;
    public WaypointGraph waypointGraph;
    public float cameraOrthoSize;
}

public class SaveLoadManager : MonoBehaviour
{
    Map map;
    Camera cam;

    private void Awake()
    {
        cam = Camera.main;
        GameObject go = GameObject.Find("MapManager");
        if (!go) return;
        map = go.GetComponent<Map>();
        map.GenerateEmpty();

        Load("Level_00");
    }

    public void Load(string name)
    {
        string path = Application.persistentDataPath + "/" + name + ".json";
        if (!File.Exists(path))
            return;

        Debug.Log("Loading " + name);

        string json = File.ReadAllText(path);
        MapSaveData save = JsonUtility.FromJson<MapSaveData>(json);
        map.data.cells.CreateFromList(save.cells, save.width, save.height);
        map.data.width = save.width;
        map.data.height = save.height;
        map.data.waypointGraph = save.waypointGraph;
        cam.orthographicSize = save.cameraOrthoSize;
    }

    public void Save(string name)
    {
        MapSaveData save = new MapSaveData
        {
            width = map.data.width,
            height = map.data.height,
            cells = map.data.cells.GetAsList(),
            waypointGraph = map.data.waypointGraph,
            cameraOrthoSize = cam.orthographicSize
        };
        string json = JsonUtility.ToJson(save, true);
        File.WriteAllText(Application.persistentDataPath + "/" + name + ".json", json);
    }
}
