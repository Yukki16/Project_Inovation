using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameManager : MonoBehaviour
{
    public class SlowDownPlayerArgs
    {
        public Player player;
    }
    public event EventHandler<SlowDownPlayerArgs> SlowDownPlayer;
    public static GameManager Instance { get; private set; }

    [SerializeField] private Transform[] players;
    [SerializeField] private ObjectSpawner objectSpawner;

    private const float MAXGAMELENGTH = 900;
    private const float MAXGAMETIME = 120;
    private const float DIFFERENCEHEIGHTFORPOWERUP = 5;

    //Spawn
    private const float MINSPAWNRATE = 0.4f;
    private const float MAXSPAWNMODIFIERTIME = 10;
    private const float MAXPOWERUPTIMER = 10;
    private const float SPAWNRATEDECREASE = 0.2f;
    private float spawnModifierTimer;
    private float maxSpawnRate = 1f;
    private float spawnRate = 1.5f;
    private float powerUpTimer;

    private void Awake()
    {
        Instance = this;
        spawnModifierTimer = MAXSPAWNMODIFIERTIME;
        spawnRate = maxSpawnRate;
        powerUpTimer = MAXPOWERUPTIMER;
    }

    public float GetLowestHeightOfAllPlayers()
    {
        float lowestHeight = players[0].position.y;
        foreach (Transform player in players)
        {
            if (player.position.y <= lowestHeight) { 
                lowestHeight = player.position.y;
            }
        }
        return lowestHeight;
    }

    public Transform GetLowestPlayer()
    {
        float lowestHeight = players[0].position.y;
        Transform lowestHeightPlayer = players[0];
        foreach (Transform player in players)
        {
            if (player.position.y <= lowestHeight)
            {
                lowestHeight = player.position.y;
                lowestHeightPlayer = player;
            }
        }
        return lowestHeightPlayer;
    }

    public float GetHighestHeightOfAllPlayers()
    {
        float highestHeight = players[0].position.y;
        foreach (Transform player in players)
        {
            if (player.position.y >= highestHeight)
            {
                highestHeight = player.position.y;
            }
        }
        return highestHeight;
    }

    public Player GetHightestHeightPlayer()
    {
        float highestHeight = players[0].position.y;
        Player playerWithHighestHeight = players[0].GetComponentInParent<Player>();
        foreach (Transform player in players)
        {
            if (player.position.y >= highestHeight)
            {
                highestHeight = player.position.y;
                playerWithHighestHeight = player.GetComponentInParent<Player>();
            }
        }
        return playerWithHighestHeight;
    }

    public float GetMaxGameLength()
    {
        return MAXGAMELENGTH;
    }

    public float GetMaxGameTime()
    {
        return MAXGAMETIME;
    }

    private void Update()
    {
        if (TCMiniGameStateManager.Instance.GameIsPlaying())
        {
            HandleSpawningOfItems();
            HandlePowerUps();
            if (GetHighestHeightOfAllPlayers() >= GetMaxGameLength())
            {
                TCMiniGameStateManager.Instance.EndGame();
            }
        }
    }

    private void SpawnFallingItems()
    {
        foreach (Transform player in players)
        {
            objectSpawner.SpawnObject(player.position);
        }
        spawnRate = maxSpawnRate;
    }

    private void SpawnPowerUps(Transform player)
    {
        objectSpawner.SpawnPowerUpItem(player.position);
        powerUpTimer = MAXPOWERUPTIMER;
    }

    private void HandlePowerUps()
    {
        powerUpTimer -= Time.deltaTime;
        if (powerUpTimer <= 0)
        {
            if (GetHighestHeightOfAllPlayers() - GetLowestHeightOfAllPlayers() >= DIFFERENCEHEIGHTFORPOWERUP)
            {
                SpawnPowerUps(GetLowestPlayer());
            }
        }
    }

    private void HandleSpawningOfItems()
    {
        spawnRate -= Time.deltaTime;
        if (spawnRate <= 0)
        {
            SpawnFallingItems();
        }

        if (spawnRate > MINSPAWNRATE)
        {
            spawnModifierTimer -= Time.deltaTime;
            if (spawnModifierTimer <= 0)
            {
                maxSpawnRate -= SPAWNRATEDECREASE;
                spawnModifierTimer = MAXSPAWNMODIFIERTIME;
            }
        }
    }

    public Dictionary<string,int> GetScoreboard()
    {
        Dictionary<string, int> scoreboardResults = new Dictionary<string,int>();
        foreach (Transform player in players)
        {
            Player currentPlayer = player.GetComponentInParent<Player>();
            scoreboardResults.Add(currentPlayer.GetNickname(), currentPlayer.GetScore());
        }
        return scoreboardResults;
    }

    public void SlowDownTopPlayer()
    {
        SlowDownPlayer?.Invoke(this, new SlowDownPlayerArgs
        {
            player = GetHightestHeightPlayer()
        });
    }
}
