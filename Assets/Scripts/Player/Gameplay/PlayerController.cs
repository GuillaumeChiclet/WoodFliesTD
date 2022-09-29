using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class PlayerController : MonoBehaviour
{
    //  VARIABLES

    Vector3 moveDirection;
    public Color playerColor { get; private set; } = Color.blue;


    [HideInInspector]
    public float dashInput;

    [HideInInspector]
    public bool isInteracting;

    PlayerInput inputs;

    public Vector3 MoveDirection {get { return moveDirection; } }

    //  MONOBEHAVIOUR FUNCTIONS

    private void Awake()
    {
        inputs = GetComponent<PlayerInput>();
    }

    private void OnEnable()
    {
        GameInstance.Instance.OnGameStart.AddListener(OnGameStart);
    }

    private void OnDisable()
    {
        GameInstance.Instance.OnGameStart.RemoveListener(OnGameStart);
    }

    //  FUNCTIONS

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

    //  OTHER EVENT FUNCTIONS

    private void OnGameStart()
    {
        if (TryGetComponent(out PlayerDecal decal)) decal.enabled = true;
    }
}
