using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.IO;

[System.Serializable]
public class CameraSaveData
{
    public float orthographicSize;
    public float height;
}


[System.Serializable]
public class GameSaveData
{
    public MapSaveData mapSaveData;
    public CameraSaveData camSaveData;
    // Players data
    // Game round data
}

public class SaveLoadManager : MonoBehaviour
{
    Map map;
    Camera cam;
    AssetsManager assets;

    private void Awake()
    {
        cam = Camera.main;
        GameObject go = GameObject.Find("MapManager");
        if (!go) return;
        map = go.GetComponent<Map>();
        map.Initialize();

        go = null;
        go = GameObject.Find("AssetsManager");
        if (!go) return;
        assets = go.GetComponent<AssetsManager>();
        assets.Initialize();


        if (PlayerPrefs.HasKey("sceneName"))
        {
            Load(PlayerPrefs.GetString("sceneName"));
        }
        else
        {
            Load("Level_00");
        }
    }

    

    public void Load(string name)
    {
        string path = Application.persistentDataPath + "/" + name + ".json";
        if (!File.Exists(path))
            return;

        Debug.Log("Loading " + name);

        string json = File.ReadAllText(path);
        GameSaveData gameSaveData = JsonUtility.FromJson<GameSaveData>(json);

        // Load everything back to same state
        map.LoadMapSaveData(gameSaveData.mapSaveData);
        cam.orthographicSize = gameSaveData.camSaveData.orthographicSize;
        cam.transform.SetPositionAndRotation(Vector3.up * gameSaveData.camSaveData.height, Quaternion.Euler(45, 45, 0));
    }

    public void Save(string name)
    {
        GameSaveData gameSaveData = new GameSaveData
        {
            mapSaveData = map.GetMapSaveData(),
            camSaveData = new CameraSaveData { orthographicSize = cam.orthographicSize, height = cam.transform.position.y }
        };

        string json = JsonUtility.ToJson(gameSaveData, true);
        File.WriteAllText(Application.persistentDataPath + "/" + name + ".json", json);
    }
}
