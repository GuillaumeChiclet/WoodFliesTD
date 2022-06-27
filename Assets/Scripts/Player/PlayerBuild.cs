using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerBuild : MonoBehaviour
{
    bool isBuilding = false;

    GameObject buildPreview = null;

    PlayerController controller;

    [SerializeField] GameObject buildingPrefab;

    public Material buildable;
    public Material nonBuildable;

    private void Start()
    {
        controller = GetComponentInChildren<PlayerController>();
    }
    void Update()
    {
        if (Input.GetKeyDown(KeyCode.B))
        {
            if (isBuilding)
                EndBuild();
            else
                BeginBuild();
        }
        if (isBuilding) 
        {
            RefreshBuildPreview();
            if (Input.GetMouseButtonDown(1))
            {
                TryBuild();
            }
        }


    }

    void BeginBuild() 
    {
        isBuilding = true;
        if (!buildPreview)
            buildPreview = Instantiate(buildingPrefab);
        
    }

    void TryBuild() 
    {
        if (!controller.CurrentCellBelow.isBuildable)
            return;

        PlaceShematic();
        EndBuild();
    }

    void PlaceShematic() 
    {
        controller.map.TrySpawnCellEntity(controller.currentCellBelowGridPos, "Shematic", out CellEntity entity);
        EndBuild();
    }

    void EndBuild() 
    {
        isBuilding = false;
        if (buildPreview)
            Destroy(buildPreview);
    }

    void RefreshBuildPreview() 
    {
        if (!buildPreview)
            return;

        Renderer[] renderers = buildPreview.GetComponentsInChildren<Renderer>();
        foreach (Renderer renderer in renderers) 
            renderer.material = controller.CurrentCellBelow.canBuild ? buildable : nonBuildable;

        buildPreview.transform.position = MapCoordinates.CellToWorldCoords(controller.currentCellBelowGridPos);
    }
}
