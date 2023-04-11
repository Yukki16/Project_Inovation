using System;
using System.Collections;
using System.Collections.Generic;
using Unity.Netcode;
using UnityEngine;

public class TCMiniGameStateManager : NetworkBehaviour
{
    [SerializeField] private Transform playerPrefab;
    public static TCMiniGameStateManager Instance { get; private set; }
    public event EventHandler OnAllPlayersJoined;
    public class GameStateChangedArgs
    {
        public GameState gameState;
    }
    public event EventHandler<GameStateChangedArgs> GameStateChanged;
    public enum GameState
    {
        WAITING,
        IN_COUNTDOWN,
        PLAYING,
        STOPPED,
        SHOWING_SCORE
    }

    private NetworkVariable<GameState> currentGameState = new NetworkVariable<GameState>(GameState.WAITING);

    private NetworkVariable<float> countdownTimer = new NetworkVariable<float>(5);
    private NetworkVariable<float> playingTime = new NetworkVariable<float>(120);
    private NetworkVariable<float> stoppedWaitTime = new NetworkVariable<float>(5);
    private NetworkVariable<float> showingScoreWaitTime = new NetworkVariable<float>(20);

    private Dictionary<ulong, bool> playerReadyDictionary;

    private void Awake()
    {
        Instance = this;
        playerReadyDictionary = new Dictionary<ulong, bool>();
    }

    public override void OnNetworkSpawn()
    {
        currentGameState.OnValueChanged += CurrentGameState_OnValueChanged;
        if (IsServer)
        {
            NetworkManager.Singleton.SceneManager.OnLoadEventCompleted += SceneManager_OnLoadEventCompleted;
        }
    }

    private void SceneManager_OnLoadEventCompleted(string sceneName, UnityEngine.SceneManagement.LoadSceneMode loadSceneMode, List<ulong> clientsCompleted, List<ulong> clientsTimedOut)
    {
        foreach (ulong clientId in NetworkManager.Singleton.ConnectedClientsIds)
        {
            Transform playerTransform = Instantiate(playerPrefab);
            GameObject.FindGameObjectWithTag("PhoneUI").gameObject.GetComponent<Canvas>().worldCamera = playerTransform.GetComponentInChildren<Camera>();
            playerTransform.GetComponent<NetworkObject>().SpawnAsPlayerObject(clientId, true);  
        }
        OnAllPlayersJoined?.Invoke(this, EventArgs.Empty);
        currentGameState.Value = GameState.IN_COUNTDOWN;
    }

    private void CurrentGameState_OnValueChanged(GameState previousValue, GameState newValue)
    {
        GameStateChanged?.Invoke(this, new GameStateChangedArgs { gameState = currentGameState.Value });
    }

    void Update()
    {
        if (!IsServer)
        {
            return;
        }
        switch (currentGameState.Value)
        {
            case GameState.IN_COUNTDOWN:
                countdownTimer.Value -= Time.deltaTime;
                if (countdownTimer.Value <= 0)
                {
                    currentGameState.Value = GameState.PLAYING;
                }
                break;
            case GameState.PLAYING:
                playingTime.Value -= Time.deltaTime;
                if (playingTime.Value <= 0)
                {
                    EndGame();
                }
                break;
            case GameState.STOPPED:
                stoppedWaitTime.Value -= Time.deltaTime;
                if (stoppedWaitTime.Value <= 0)
                {
                    currentGameState.Value = GameState.SHOWING_SCORE; 
                }
                break;
            case GameState.SHOWING_SCORE:
                showingScoreWaitTime.Value -= Time.deltaTime;
                if (showingScoreWaitTime.Value <= 0)
                {
                    //TODO
                }
                break;
        }
    }

    public bool GameIsWaiting()
    {
        return currentGameState.Value == GameState.WAITING;
    }

    public bool GameIsInCountdown()
    {
        return currentGameState.Value == GameState.IN_COUNTDOWN;
    }
    public float GetCountdown()
    {
        return countdownTimer.Value;
    }
    public bool GameIsPlaying()
    {
        return currentGameState.Value == GameState.PLAYING;
    }
    public bool GameIsFinished()
    {
        return currentGameState.Value == GameState.STOPPED;
    }
    public void EndGame()
    {
        if (GameIsPlaying())
        {
            currentGameState.Value = GameState.STOPPED;
        }
    }
}
