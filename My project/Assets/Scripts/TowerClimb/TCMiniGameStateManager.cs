using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Mirror;

public class TCMiniGameStateManager : NetworkBehaviour
{
    public static TCMiniGameStateManager Instance { get; private set; }
    
    public class GameStateChangedArgs
    {
        [SyncVar] public GameState gameState;
    }
    public event EventHandler<GameStateChangedArgs> GameStateChanged;
    public event EventHandler CountdownStopped;
    public enum GameState
    {
        IN_COUNTDOWN,
        PLAYING,
        STOPPED,
        SHOWING_SCORE,
        NOT_READY
    }

    public GameState currentGameState;

    private float countdownTimer = 5;
    private float playingTime;
    private float stoppedWaitTime = 5;
    private float showingScoreWaitTime = 20;

    private void Awake()
    {
        Instance = this;
        currentGameState = GameState.NOT_READY;
    }

    private void Start()
    {
        playingTime = GameManager.Instance ? GameManager.Instance.GetMaxGameTime() : 60;
    }

    void Update()
    {
        if(Input.GetKeyDown(KeyCode.O))
        {
            currentGameState = GameState.IN_COUNTDOWN;
            Debug.Log("Started Game");
        }
        switch (currentGameState)
        {
            case GameState.IN_COUNTDOWN:
                countdownTimer -= Time.deltaTime;
                if (countdownTimer <= 0)
                {
                    currentGameState = GameState.PLAYING;
                    GameStateChanged?.Invoke(this, new GameStateChangedArgs { gameState = currentGameState });
                    CountdownStopped?.Invoke(this, EventArgs.Empty);
                    Debug.Log("Countdown ended");
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
                    currentGameState = GameState.SHOWING_SCORE; 
                    GameStateChanged?.Invoke(this, new GameStateChangedArgs { gameState = currentGameState });
                }
                break;
            case GameState.SHOWING_SCORE:
                showingScoreWaitTime -= Time.deltaTime;
                if (showingScoreWaitTime <= 0)
                {
                    //TODO
                }
                break;
        }
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
    public void EndGame()
    {
        if (GameIsPlaying())
        {
            currentGameState = GameState.STOPPED;
            GameStateChanged?.Invoke(this, new GameStateChangedArgs { gameState = currentGameState });
        }
    }
}
