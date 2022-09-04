using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LobbySlot : MonoBehaviour
{
    private int droneIndex = 0;
    private AssetDatabase<ScriptableDrone> DroneDatabase;

    private GameObject preview;

    void OnEnable()
    {
        ChangeDronePreview();
    }

    private void OnDisable()
    {
        if (preview) Destroy(preview);
        droneIndex = 0;
    }

    private void ChangeDronePreview()
    {
        if (preview) Destroy(preview);
        preview = Instantiate(GameInstance.Instance.database.DroneDatabase.list[droneIndex].model, transform);
    }

    public void ChangeCurrentDrone(int direction)
    {
        droneIndex = (int)Mathf.Repeat(droneIndex + direction, GameInstance.Instance.database.DroneDatabase.list.Count);
        ChangeDronePreview();
    }

    public ScriptableDrone GetCurrentDrone()
    {
        return GameInstance.Instance.database.DroneDatabase.list[droneIndex];
    }
}
