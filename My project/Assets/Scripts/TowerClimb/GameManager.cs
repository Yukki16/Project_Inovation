using System;
using System.Collections;
using System.Collections.Generic;
using Unity.Netcode;
using UnityEngine;

public class GameManager : NetworkBehaviour
{
    public class SlowDownPlayerArgs
    {
        public Player player;
    }
    public event EventHandler<SlowDownPlayerArgs> SlowDownPlayer;
    public static GameManager Instance { get; private set; }

    [SerializeField] private List<Transform> players;
    [SerializeField] private Transform cameraPrefab;

    private const float MAXGAMELENGTH = 900;
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
        players = new List<Transform>();
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

    private void Update()
    {
        if (IsHost)
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
    }

    private void SpawnFallingItems()
    {
        foreach (Transform player in players)
        {
            ObjectSpawner.Instance.SpawnObject(player.position);
        }
        spawnRate = maxSpawnRate;
    }

    private void SpawnPowerUps(Transform player)
    {
        ObjectSpawner.Instance.SpawnPowerUpItem(player.position);
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

    /// <summary>
    /// Adds a player to the player list.
    /// </summary>
    /// <param name="playerBody"></param>
    /// <param name="player"></param>
    /// <returns>Amount of degrees the player should spawn at.</returns>
    public int AddPlayer(Transform playerBody, Player player)
    {
        if (!players.Contains(playerBody))
        {
            players.Add(playerBody);
        }
        
        //Transform spawnedCamera = Instantiate(cameraPrefab, transform.position, transform.rotation);
        //spawnedCamera.transform.Rotate(0, 90 * (players.Count - 1), 0);
        //player.ModifyCamera(spawnedCamera.gameObject);
        return 90 * (players.Count - 1);
    }
}
