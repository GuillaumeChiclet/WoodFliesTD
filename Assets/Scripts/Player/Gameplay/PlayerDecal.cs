using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(PlayerController))]
public class PlayerDecal : MonoBehaviour
{
    [Header("Layers")]

    [HideInInspector] private LayerMask terrainLayerMask;

    [Header("Model")]

    [SerializeField] private GameObject decalModel;
    private GameObject decalTarget;
    private float decalOffset = 0.1f;

    private PlayerController cont;
    private Vector3 hitPoint;

    //  Cell marker
    [HideInInspector] public Map map = null;
    [HideInInspector] public Vector2Int currentCellBelowGridPos;

    private Cell currentCellBelow = null;
    public Cell CurrentCellBelow => currentCellBelow;

    private void Awake()
    {
        terrainLayerMask = LayerMask.GetMask("Terrain");
        cont = GetComponent<PlayerController>();
    }

    void OnEnable()
    {
        decalTarget = Instantiate(decalModel, null);
        map = GameObject.Find("MapManager").GetComponent<Map>();
        Material decalMat = decalTarget.GetComponent<MeshRenderer>().material;
        decalMat.color = cont.playerColor;
    }

    private void OnDisable()
    {
        Destroy(decalTarget);
    }


    void Update()
    {
        if (cont.isInteracting) 
        {
            currentCellBelow.ownedEntity?.GetComponent<IInteractable>().PrimarAction(gameObject);
        }
    }

    private void FixedUpdate()
    {
        UpdateCellBelow();
        UpdateDecalTarget();
    }

    void UpdateCellBelow()
    {
        bool touch = Physics.SphereCast(transform.position, 0.1f, Vector3.down, out RaycastHit hit, 50, terrainLayerMask);
        if (touch)
        {
            hitPoint = hit.point;
            map.TryGetCellFromWorldPos(hitPoint, out currentCellBelow);
            MapCoordinates.WorldToCellCoords(hitPoint, ref currentCellBelowGridPos);
        }
    }

    void UpdateDecalTarget()
    {
        if (currentCellBelow == null) return;

        int x = 0, y = 0;
        MapCoordinates.WorldToCellCoords(hitPoint, ref x, ref y);

        Vector3 pos = new Vector3(x * MapCoordinates.unitSize, currentCellBelow.height + decalOffset, y * MapCoordinates.unitSize);
        Debug.Log(pos);
        //decalTarget.transform.position = Vector3.MoveTowards(decalTarget.transform.position, new Vector3(x * MapCoordinates.unitSize, currentCellBelow.height, y * MapCoordinates.unitSize), Time.deltaTime * 5.0f);
        decalTarget.transform.position = pos;
    }
}
