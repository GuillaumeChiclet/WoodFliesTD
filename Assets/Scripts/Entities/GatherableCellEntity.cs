using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GatherableCellEntity : CellEntity, IInteractable
{
   
    [SerializeField] ScriptableResource resource;

    public float respawnTime   = 10f;
    public float gatheringTime = 2f;

    public int resourceField   = 50;
    int   resourceLeft         = 0;

    public bool Empty     => resourceLeft <= 0;
    public int ResourceID => resource.ID;
    // Gather resources
    public void PrimarAction(GameObject caller)
    {
        caller.GetComponent<PlayerGather>()?.StartGathering(this);
    }

    // Any other action
    public void SecondAction(GameObject caller)
    {
        throw new System.NotImplementedException();
    }

    public void StartRespawnPhase() => StartCoroutine(RespawnPhase());
    private void Start()
    {
        resourceLeft = resourceField;
        cell = GameObject.Find("MapManager").GetComponent<Map>().GetCellFromWorldPos(transform.position);
        cell.ownedEntity = this;
    }

    public void Decrement() 
    {
        resourceLeft--;
        if (Empty) 
            StartRespawnPhase();
    }

    IEnumerator RespawnPhase() 
    {
        float elapsedTime = 0f;
        while (elapsedTime >= respawnTime) 
        {
            elapsedTime += Time.deltaTime;
            yield return null;
        }

        resourceLeft = resourceField;
    }        
}
