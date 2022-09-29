using System.Collections;
using System.Collections.Generic;
using UnityEngine.InputSystem;
using UnityEngine.SceneManagement;
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
        inputsManager = GetComponent<PlayerInputManager>();
        playerConfig = new List<PlayerConfiguration>();
    }

    public void PlayerReady(int index)
    {
        playerConfig[index].IsReady = true;
        playerConfig[index].Drone = slots[index].GetCurrentDrone();

        //  Make the player don't destroy on load
        playerConfig[index].Input.transform.SetParent(transform);

        if (playerConfig.Count > 0 && playerConfig.Count <= inputsManager.maxPlayerCount && playerConfig.TrueForAll(p => p.IsReady))
        {
            inputsManager.DisableJoining();

            playerConfig.ForEach(p => { p.Input.SwitchCurrentActionMap("Gameplay"); Debug.Log(p.Input.currentActionMap.ToString()); });
            Debug.Log("All Players are ready");

            ConstructPlayers();

            //  Start ?
            SceneManager.LoadScene("GAME");
        }
    }

    public void ConstructPlayers()
    {
        playerConfig.ForEach(p => p.Input.gameObject.GetComponent<PlayerController>().ConstructPlayer(p.Drone));
    }

    public void HandlePlayerJoin(PlayerInput playerInput)
    {
        //  Avoid player duplications
        if (!playerConfig.Exists(p => p.PlayerIndex == playerInput.playerIndex))
        {
            Debug.Log("Player \"" + playerInput.playerIndex + "\" joined");

            AddPlayer(playerInput);
        }
    }

    public void AddPlayer(PlayerInput playerInput)
    {
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
        slots[index].ChangeCurrentDrone(direction);
        //  switch List of drone here
    }

    public List<GameObject> GetPlayersObjects()
    {
        List<GameObject> players = new List<GameObject>();

        playerConfig.ForEach(p => players.Add(p.Input.gameObject));

        return players;
    }
}




public class PlayerConfiguration
{
    public PlayerConfiguration(PlayerInput playerInput)
    {
        Debug.Log(playerInput.playerIndex + "joined");

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