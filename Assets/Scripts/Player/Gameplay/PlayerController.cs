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

    //  Cell marker
    public Map map = null;
    Cell currentCellBelow = null;
    public Vector2Int currentCellBelowGridPos;

    public Cell CurrentCellBelow => currentCellBelow;
    Vector3 hitPoint;

    [HideInInspector]
    public float dashInput;

    [HideInInspector]
    public bool isInteracting;

    PlayerInput inputs;

    public Vector3 MoveDirection {get { return moveDirection; } }

    private void Awake()
    {
        inputs = GetComponent<PlayerInput>();
    }

    // Start is called before the first frame update
    void Start()
    {
        /*map = GameObject.Find("MapManager").GetComponent<Map>();
        Material decalMat = decalTarget.GetComponent<MeshRenderer>().material;
        decalMat.color = playerColor;*/
    }

    // Update is called once per frame
    void Update()
    {
        //UpdateInputDir();

        /*if (isInteracting) 
        {
            currentCellBelow.ownedEntity?.GetComponent<IInteractable>().PrimarAction(gameObject);
        }*/
    }

    private void FixedUpdate()
    {
        //UpdateCellBelow();
        //UpdateDecalTarget();
    }

    void UpdateCellBelow()
    {
        /*bool touch = Physics.SphereCast(gameObject.transform.position, 0.1f, Vector3.down, out RaycastHit hit, 50, terrainLayerMask);
        if (touch)
        {
            hitPoint = hit.point;
            map.TryGetCellFromWorldPos(hitPoint, out currentCellBelow);
            MapCoordinates.WorldToCellCoords(hitPoint, ref currentCellBelowGridPos);
        }*/
    }

    void UpdateDecalTarget()
    {
        /*if (currentCellBelow == null)
            return;

        int x = 0, y = 0;
        MapCoordinates.WorldToCellCoords(hitPoint, ref x, ref y);
        //decalTarget.transform.position = Vector3.MoveTowards(decalTarget.transform.position, new Vector3(x * MapCoordinates.unitSize, currentCellBelow.height, y * MapCoordinates.unitSize), Time.deltaTime * 5.0f);
        decalTarget.transform.position = new Vector3(x * MapCoordinates.unitSize, currentCellBelow.height + decalOffset, y * MapCoordinates.unitSize);*/
    }


    public void ConstructPlayer(ScriptableDrone drone)
    {

        PlayerPawn pawn = gameObject.GetComponent<PlayerPawn>();
        pawn.drone = drone;
        pawn.enabled = true;

        inputs.SwitchCurrentActionMap("Gameplay");
    }

    //  GAMEPLAY ACTIONS

    public void OnMove(InputAction.CallbackContext context)
    {
        Vector2 dir = context.ReadValue<Vector2>();
        moveDirection = new Vector3(dir.x, 0.0f, dir.y);
    }

    public void OnDash(InputAction.CallbackContext context) => dashInput = context.ReadValue<float>();
    public void OnInteract(InputAction.CallbackContext context) => isInteracting = context.ReadValue<bool>();


    //  LOBBY ACTIONS


    public int lobbyIndex = -1;
    public void OnDroneChange(InputAction.CallbackContext context)
    {
        if (context.started) GameInstance.Instance.playerConfigs.ChangeDrone(lobbyIndex, (int)Mathf.Sign(context.ReadValue<float>()));
    }

    public void OnGetReady(InputAction.CallbackContext context)
    {
        if (context.started) GameInstance.Instance.playerConfigs.PlayerReady(lobbyIndex);
    }

    public void OnLeave(InputAction.CallbackContext context)
    {
        if (context.started) GameInstance.Instance.playerConfigs.RemovePlayer(lobbyIndex);
    }
}
