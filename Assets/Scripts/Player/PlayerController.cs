using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class PlayerController : MonoBehaviour
{
    [Header("Model")]
    public GameObject decalTarget;
    private float decalOffset = 0.1f;

    [Header("Layers")]
    public LayerMask terrainLayerMask;

    Vector3 moveDirection;
    public Color playerColor = Color.blue;

    public Map map = null;
    Cell currentCellBelow = null;
    public Vector2Int currentCellBelowGridPos;

    public Cell CurrentCellBelow => currentCellBelow;
    Vector3 hitPoint;

    [HideInInspector]
    public float dashInput;

    [HideInInspector]
    public bool isInteracting;

    public Vector3 MoveDirection {get { return moveDirection; } }

    // Start is called before the first frame update
    void Start()
    {
        map = GameObject.Find("MapManager").GetComponent<Map>();
        Material decalMat = decalTarget.GetComponent<MeshRenderer>().material;
        decalMat.color = playerColor;
    }

    // Update is called once per frame
    void Update()
    {
        //UpdateInputDir();

        /*if (Input.GetKeyDown(KeyCode.E)) 
        {
            currentCellBelow.ownedEntity?.GetComponent<IInteractable>().PrimarAction(gameObject);
        }

        dash = Input.GetKeyDown(KeyCode.Space);*/
    }

    private void FixedUpdate()
    {
        UpdateCellBelow();
        UpdateDecalTarget();
    }

    void UpdateCellBelow()
    {
        bool touch = Physics.SphereCast(gameObject.transform.position, 0.1f, Vector3.down, out RaycastHit hit, 50, terrainLayerMask);
        if (touch)
        {
            hitPoint = hit.point;
            map.TryGetCellFromWorldPos(hitPoint, out currentCellBelow);
            MapCoordinates.WorldToCellCoords(hitPoint, ref currentCellBelowGridPos);
        }
    }

    void UpdateDecalTarget()
    {
        if (currentCellBelow == null)
            return;

        int x = 0, y = 0;
        MapCoordinates.WorldToCellCoords(hitPoint, ref x, ref y);
        //decalTarget.transform.position = Vector3.MoveTowards(decalTarget.transform.position, new Vector3(x * MapCoordinates.unitSize, currentCellBelow.height, y * MapCoordinates.unitSize), Time.deltaTime * 5.0f);
        decalTarget.transform.position = new Vector3(x * MapCoordinates.unitSize, currentCellBelow.height + decalOffset, y * MapCoordinates.unitSize);
    }

    /* void UpdateInputDir()
     {
         moveDirection = new Vector3(Input.GetAxis("Horizontal"), 0, Input.GetAxis("Vertical"));
         moveDirection = moveDirection.magnitude > 1.0f ? moveDirection.normalized : moveDirection;
         moveDirection = Quaternion.AngleAxis(45, Vector3.up) * moveDirection;
     }*/

    public void OnMove(InputAction.CallbackContext context)
    {
        Vector2 dir = context.ReadValue<Vector2>();
        moveDirection = new Vector3(dir.x, 0.0f, dir.y);
    }

    public void OnDash(InputAction.CallbackContext context) => dashInput = context.ReadValue<float>();
    public void OnInteract(InputAction.CallbackContext context) => isInteracting = context.ReadValue<bool>();

}
