using System.Collections;
using System.Collections.Generic;
using UnityEngine.InputSystem;
using UnityEngine;

[RequireComponent(typeof(PlayerInputManager))]
public class PlayerConfigurationManager : MonoBehaviour
{
    private List<PlayerConfiguration> playerConfig;
    private PlayerInputManager inputsManager;

    //public static PlayerConfigurationManager Instance { get; private set; }
    public List<LobbySlot> slots;

    private void Awake()
    {
        //if(Instance != null)
        //{
        //    Debug.LogWarning("Trying to create another instance of singleton !");
        //}
        //else
        //{
        //    Instance = this;
        //    DontDestroyOnLoad(Instance);

            inputsManager = GetComponent<PlayerInputManager>();
            playerConfig  = new List<PlayerConfiguration>();
        //}
    }

    public void PlayerReady(int index)
    {
        playerConfig[index].IsReady = true;
        playerConfig[index].Drone = slots[index].GetCurrentDrone();

        if (playerConfig.Count > 0 && playerConfig.Count <= inputsManager.maxPlayerCount && playerConfig.TrueForAll(p => p.IsReady))
        {
            inputsManager.DisableJoining();
            
            Debug.Log("All Players are ready");

            playerConfig.ForEach(p => p.Input.SwitchCurrentActionMap("Gameplay"));

            ConstructPlayers();
            //  Start ?
        }
    }

    public void ConstructPlayers()
    {
        playerConfig.ForEach(p => p.Input.gameObject.GetComponent<PlayerController>().ConstructPlayer(p.Drone));
    }

    public void HandlePlayerJoin(PlayerInput playerInput)
    {
        if (!playerConfig.Exists(p => p.PlayerIndex == playerInput.playerIndex))
        {
            Debug.Log("Player \"" + playerInput.playerIndex + "\" joined");

            AddPlayer(playerInput);
        }
    }

    public void AddPlayer(PlayerInput playerInput)
    {
        playerInput.transform.SetParent(transform);
        playerInput.gameObject.GetComponent<PlayerController>().lobbyIndex = playerConfig.Count;
        slots[playerConfig.Count].enabled = true;

        playerConfig.Add(new PlayerConfiguration(playerInput));

    }

    public void RemovePlayer(int index)
    {
        playerConfig.Remove(playerConfig[index]);

        //  Reset lobby indexes
        //  TODO : try it or clean it, maybe index changes is already handled by the input system
        int current = 0;
        foreach(PlayerConfiguration pc in playerConfig)
        {
            PlayerController controller = pc.Input.gameObject.GetComponent<PlayerController>();

            if (controller.lobbyIndex == current) continue;
            controller.lobbyIndex = current;

            current++;
        }
    }

    public void ChangeDrone(int index, int direction)
    {
        Debug.Log("Switch drone");
        slots[index].ChangeCurrentDrone(direction);
        //  switch List of drone here
    }
}




public class PlayerConfiguration
{
    public PlayerConfiguration(PlayerInput playerInput)
    {
        PlayerIndex = playerInput.playerIndex;
        Input = playerInput;

        playerInput.SwitchCurrentActionMap("Lobby");
    }

    public PlayerInput Input { get; set; }
    public int PlayerIndex   { get; set; }
    public bool IsReady      { get; set; }

    public ScriptableDrone Drone   { get; set; }
    public Material PlayerMaterial { get; set; }
}