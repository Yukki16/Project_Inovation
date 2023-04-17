using Assets.Scripts;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class GlidingGameManager : MonoBehaviour
{
    [SerializeField] private int gameLength;
    private List<Transform> players;
    public static GlidingGameManager Instance { get; private set; }
    public class GameStateChangedArgs
    {
        public GameState gameState;
    }
    public event EventHandler<GameStateChangedArgs> GameStateChanged;
    public enum GameState
    {
        WAITING,
        SHOWING_TUTORIAL,
        IN_COUNTDOWN,
        PLAYING,
        STOPPED,
    }

    private GameState currentGameState;
    private float showingTutorialTimer = 8;
    private float countdownTimer = 5;
    private float playingTime = 20;
    private float stoppedWaitTime = 7;

    private void Awake()
    {
        Instance = this;
        currentGameState = GameState.WAITING;
        NetworkManager.Instance.OnAllPlayersJoined += Instance_OnAllPlayersJoined;
    }

    private void Instance_OnAllPlayersJoined(object sender, EventArgs e)
    {
        StartGame();
    }

    void Update()
    {
        if (GetFurthestDistanceOfAllPlayers() >= -65 && GameIsPlaying())
        {
            EndGame();
        }
        switch (currentGameState)
        {
            case GameState.SHOWING_TUTORIAL:
                showingTutorialTimer -= Time.deltaTime;
                if (showingTutorialTimer <= 0)
                {
                    currentGameState = GameState.IN_COUNTDOWN;
                    GameStateChanged?.Invoke(this, new GameStateChangedArgs { gameState = currentGameState });
                }
                break;
            case GameState.IN_COUNTDOWN:
                countdownTimer -= Time.deltaTime;
                if (countdownTimer <= 0)
                {
                    NetworkManager.Instance.NotifyClientsForGameStart();
                    currentGameState = GameState.PLAYING;
                    GameStateChanged?.Invoke(this, new GameStateChangedArgs { gameState = currentGameState });
                }
                break;
            case GameState.PLAYING:
                playingTime -= Time.deltaTime;
                if (playingTime <= 0)
                {
                    EndGame();
                }
                break;
            case GameState.STOPPED:
                stoppedWaitTime -= Time.deltaTime;
                if (stoppedWaitTime <= 0)
                {
                    GeneralGameManager.Instance.EndMinigame();
                }
                break;
        }
    }

    public bool GameIsWaiting()
    {
        return currentGameState == GameState.WAITING;
    }

    public bool GameIsInCountdown()
    {
        return currentGameState == GameState.IN_COUNTDOWN;
    }
    public float GetCountdown()
    {
        return countdownTimer;
    }
    public bool GameIsPlaying()
    {
        return currentGameState == GameState.PLAYING;
    }
    public bool GameIsFinished()
    {
        return currentGameState == GameState.STOPPED;
    }

    public void StartGame()
    {
        if (GameIsWaiting())
        {
            currentGameState = GameState.SHOWING_TUTORIAL;
            GameStateChanged?.Invoke(this, new GameStateChangedArgs { gameState = currentGameState });
        }
    }

    public void EndGame()
    {
        if (GameIsPlaying())
        {
            currentGameState = GameState.STOPPED;
            GameStateChanged?.Invoke(this, new GameStateChangedArgs { gameState = currentGameState });
        }
    }

    public int GetGameLengthInMeters()
    {
        return gameLength;
    }
    public float GetFurthestDistanceOfAllPlayers()
    {
        GetAndUpdatePlayers();
        float furthestDistance = players[0].position.x;
        foreach (Transform player in players)
        {
            if (player.position.x >= furthestDistance)
            {
                furthestDistance = player.position.x;
            }
        }
        return furthestDistance;
    }


    public List<IPlayer> GetScoreboard()
    {
        GetAndUpdatePlayers();
        var winners = players.OrderByDescending(player => player.transform.position.y).ToList();
        List<IPlayer> playerWinners = new List<IPlayer>();
        foreach (var winner in winners)
        {
            playerWinners.Add(winner.GetComponent<IPlayer>());
        }
        return playerWinners;
    }

    private void GetAndUpdatePlayers()
    {
        List<Transform> newPlayerList = new List<Transform>();
        foreach (var client in NetworkManager.Instance.GetAllPlayers())
        {
            newPlayerList.Add((client as PlayerCharacter).transform);
        }
        players = newPlayerList;
    }

    public GameState ReturnCurrentGameState()
    {
        return currentGameState;
    }
}
