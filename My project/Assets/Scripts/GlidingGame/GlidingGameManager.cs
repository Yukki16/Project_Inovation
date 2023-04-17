using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GlidingGameManager : MonoBehaviour
{
    [SerializeField] private int gameLength;
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
    }

    public GameState ReturnCurrentGameState()
    {
        return currentGameState;
    }

    void Update()
    {
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
}
