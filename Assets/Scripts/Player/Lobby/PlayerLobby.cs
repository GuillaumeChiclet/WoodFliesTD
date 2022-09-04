using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;


//[RequireComponent(typeof(PlayerPawn))]
public class PlayerLobby : MonoBehaviour
{
    //PlayerPawn pawn;
    ScriptableDrone currentDrone;
    /*public int lobbyIndex = -1;

    private void Awake()
    {

    }

    public void ChangeDrone(InputAction.CallbackContext context)
    {
       //PlayerConfigurationManager.Instance.
        if (context.started)
        {
            GameInstance.Instance.playerConfigs.ChangeDrone(lobbyIndex, (int)Mathf.Sign(context.ReadValue<float>()));
        }
    }

    public void GetReady(InputAction.CallbackContext context)
    {
        if (context.started)
        {
            //pawn.drone = currentDrone;
            GameInstance.Instance.playerConfigs.PlayerReady(lobbyIndex);
        }
    }

    public void Leave(InputAction.CallbackContext context)
    {
        if (context.started)
        {
            GameInstance.Instance.playerConfigs.RemovePlayer(lobbyIndex);
        }
    }*/
}
