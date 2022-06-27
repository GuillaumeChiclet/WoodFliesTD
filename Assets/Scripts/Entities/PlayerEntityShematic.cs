using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ResourceCost
{
    public ResourceCost(ScriptableResource res, int Cost) 
    {
        resource = res;
        cost     = Cost;
    }

    public ScriptableResource resource;
    public int                cost;
}

public class PlayerEntityShematic : PlayerCellEntity, IInteractable
{
    Map map;

    [SerializeField] ScriptableCostData data;
    List<ResourceCost> resourcesPrice = new List<ResourceCost>();

    void FillResourcesCost() 
    {
        foreach (ScriptableResource resource in data.resourceList) 
        {
            resourcesPrice.Add(new ResourceCost(resource, data.resourceListCost[resource.ID]));
        }
    }

    private void Start()
    {
        map = GameObject.Find("MapManager").GetComponent<Map>();
        FillResourcesCost();
    }

    public void TryPay(PlayerGather inventory)
    {

        foreach (ResourceCost resource in resourcesPrice)
        {
            int resourceID = resource.resource.ID;
            if (inventory.resources[resourceID] >= resourcesPrice[resourceID].cost)
            {
                inventory.resources[resourceID] -= resourcesPrice[resourceID].cost;
                resourcesPrice.Remove(resource);
                inventory.RefreshUI();
                TryPay(inventory);
            }

            else if (inventory.resources[resourceID] > 0)
            {
                resourcesPrice[resourceID].cost -= inventory.resources[resourceID];
                inventory.resources[resourceID] = 0;
                inventory.RefreshUI();
                return;
            }
            return;
        }

        map.ReplaceCellEntity(MapCoordinates.WorldToCellCoords(transform.position), "Turret", out CellEntity turret);
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
