using System;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.SceneManagement;

public class GeneralGameManager : MonoBehaviour
{
    public static GeneralGameManager Instance { get; private set; }
    public enum ServerStates
    {
        IN_LOBBY,
        IN_GAME_SELECTING,
        IN_GAME,
        IN_SCOREBOARD,
        IN_END_SCREEN
    }

    public enum Minigames
    {
        NOT_PLAYABLE,
        TOWER_CLIMB,
        LETSGLIDE
    }

    private ServerStates currentState;
    private Minigames previousSelectedMinigame = Minigames.NOT_PLAYABLE;
    private Minigames currentSelectedMinigame = Minigames.NOT_PLAYABLE;

    private void Awake()
    {
        Instance = this;
        currentState = ServerStates.IN_LOBBY;
        DontDestroyOnLoad(gameObject);
    }

    private void Start()
    {
        SceneManager.sceneLoaded += SceneManager_sceneLoaded;
    }

    private void SceneManager_sceneLoaded(Scene arg0, LoadSceneMode arg1)
    {
        OnSceneLoaded();
    }

    private void OnSceneLoaded()
    {
        if (SceneManager.GetActiveScene().name == "TowerClimb")
        {
            NetworkManager.Instance.AssignMinigameCharacters(Minigames.TOWER_CLIMB);
        }
        else if (SceneManager.GetActiveScene().name == "Gliding")
        {
            NetworkManager.Instance.AssignMinigameCharacters(Minigames.LETSGLIDE);
        }
    }

    public ServerStates GetCurrentServerState()
    {
        return currentState;
    }

    public void HandleServerDisconnect()
    {
        Destroy(gameObject);
    }

    private void SelectMinigame()
    {
        List<Minigames> allMinigames = Enum.GetValues(typeof(Minigames)).Cast<Minigames>().ToList();
        bool gameSelected = false;
        do
        {
            Minigames newMinigame = allMinigames[UnityEngine.Random.Range(1, allMinigames.Count)];
            if (previousSelectedMinigame == Minigames.NOT_PLAYABLE || newMinigame != previousSelectedMinigame)
            {
                previousSelectedMinigame = currentSelectedMinigame;
                currentSelectedMinigame = newMinigame;
                currentState = ServerStates.IN_GAME_SELECTING;
                gameSelected = true;
            }
        } while (!gameSelected);      
    }

    public Minigames GetCurrentChosenMinigame()
    {
        return currentSelectedMinigame;
    }

    public void StartGame()
    {
        SelectMinigame();
        NetworkManager.Instance.LoadNetwork(Loader.Scene.GameSelecting);
    }

    public void LoadMinigame()
    {
        currentState = ServerStates.IN_GAME;
        
        switch (currentSelectedMinigame)
        {
            case Minigames.TOWER_CLIMB:
                NetworkManager.Instance.LoadNetwork(Loader.Scene.TowerClimb);
                break;
            case Minigames.LETSGLIDE:
                NetworkManager.Instance.LoadNetwork(Loader.Scene.TowerClimb);
                //NetworkManager.Instance.LoadNetwork(Loader.Scene.Gliding);
                break;
        }
        
    }
}
