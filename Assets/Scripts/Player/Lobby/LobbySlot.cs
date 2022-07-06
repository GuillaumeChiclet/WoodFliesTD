using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LobbySlot : MonoBehaviour
{
    private int droneIndex = 0;
    private DroneDatabase droneDatabase;

    private GameObject preview;

    void Start()
    {
        droneDatabase = GameInstance.Instance.database.droneDatabase;
    }

    void OnEnable()
    {
        Debug.Log("Slot Enabled");
        ChangeDronePreview();
    }

    private void OnDisable()
    {
        if (preview) Destroy(preview);
        droneIndex = 0;
    }

    private void ChangeDronePreview()
    {
        if(preview) Destroy(preview);

        preview = Instantiate(droneDatabase.allDrones[droneIndex].model, transform);
    }

    public void ChangeCurrentDrone(int direction)
    {
        droneIndex = (int)Mathf.Repeat(droneIndex + direction, droneDatabase.allDrones.Count);
        Debug.Log(direction + " value -> " + droneIndex);
        ChangeDronePreview();
    }

    public ScriptableDrone GetCurrentDrone()
    {
        return droneDatabase.allDrones[droneIndex];
    }
}
