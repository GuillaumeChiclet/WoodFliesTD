using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerEntityShematic : PlayerCellEntity, IInteractable
{

    GameObject shematicPreview;
    // need to find a way to dynamic link prefab
    GameObject shematicPrefab;

    PlayerController controller;

    private void Start()
    {
        controller = GetComponentInParent<PlayerController>();
    }

    public void TryPay(PlayerGather inventory)
    {
        // need to find a way to dynamic link costs
        ScriptableCostData data;
        /*
        foreach (ScriptableResource resource in data.resourceList)
        {
            if (data.resourceListCost[resource.ID] <= 0)
            {
                Debug.Log(resource.name + "is paid");
                continue;
            }

            if (inventory.resources[resource.ID] >= data.resourceListCost[resource.ID])
            {
                data.resourceListCost[resource.ID] = 0;
                inventory.resources[resource.ID] -= data.resourceListCost[resource.ID];
                Debug.Log(resource.name + "has been paid");
            }

            else if (inventory.resources[resource.ID] > 0)
            {
                data.resourceListCost[resource.ID] -= inventory.resources[resource.ID];
                Debug.Log(data.resourceListCost[resource.ID] + " " + resource.name + "left to pay");
                inventory.resources[resource.ID] = 0;
                return;
            }
        }*/
    }

    public void PrimarAction(GameObject caller)
    {
        TryPay(caller.GetComponent<PlayerGather>());
    }

    public void SecondAction(GameObject caller)
    {
        throw new System.NotImplementedException();
    }
}
