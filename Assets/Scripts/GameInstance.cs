using System.Collections;
using System.Collections.Generic;
using UnityEngine.Events;
using UnityEngine;

[RequireComponent(typeof(PlayerConfigurationManager))]
public class GameInstance : MonoBehaviour
{
    [HideInInspector]
    public PlayerConfigurationManager playerConfigs;

    public Database database;
    public Map currentMap;

    public UnityEvent OnGameStart;


    public static GameInstance Instance { get; private set; }

    private void Awake()
    {
        if (Instance != null)
        {
            Debug.LogWarning("Trying to create another instance of singleton !");
        }
        else
        {
            Instance = this;
            Initialize();

            DontDestroyOnLoad(Instance);
        }
    }

    private void Initialize()
    {
        database      = GetComponent<Database>();
        playerConfigs = GetComponent<PlayerConfigurationManager>();
    }

    public void MapGenerated()
    {
        OnGameStart.Invoke();
    }
}
