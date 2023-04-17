using Assets.Scripts;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Net.Sockets;
using UnityEngine;
using static GeneralGameManager;
using static TCMiniGameStateManager;

public class GameManager : MonoBehaviour
{
    public class SlowDownPlayerArgs
    {
        public TCPLayer player;
    }
    public event EventHandler<SlowDownPlayerArgs> SlowDownPlayer;
    public static GameManager Instance { get; private set; }

    private List<Transform> players;
    [SerializeField] private Transform cameraPrefab;

    private const float MAXGAMELENGTH = 766;
    private const float DIFFERENCEHEIGHTFORPOWERUP = 5;

    //Spawn
    private const float MINSPAWNRATE = 0.4f;
    private const float MAXSPAWNMODIFIERTIME = 15;
    private const float MAXPOWERUPTIMER = 10;
    private const float SPAWNRATEDECREASE = 0.2f;
    private float spawnModifierTimer;
    private float maxSpawnRate = 1f;
    private float spawnRate = 1f;
    private float powerUpTimer;

    private void Awake()
    {
        Instance = this;
        spawnModifierTimer = MAXSPAWNMODIFIERTIME;
        spawnRate = maxSpawnRate;
        powerUpTimer = MAXPOWERUPTIMER;
        players = new List<Transform>();
        NetworkManager.Instance.OnAllPlayersJoined += Instance_OnAllPlayersJoined;
    }

    private void Instance_OnAllPlayersJoined(object sender, EventArgs e)
    {
        TCMiniGameStateManager.Instance.StartGame();
    }

    public float GetLowestHeightOfAllPlayers()
    {
        GetAndUpdatePlayers();
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
        GetAndUpdatePlayers();
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
        GetAndUpdatePlayers();
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

    public TCPLayer GetHightestHeightPlayer()
    {
        GetAndUpdatePlayers();
        float highestHeight = players[0].position.y;
        TCPLayer playerWithHighestHeight = players[0].GetComponentInParent<TCPLayer>();
        foreach (Transform player in players)
        {
            if (player.position.y >= highestHeight)
            {
                highestHeight = player.position.y;
                playerWithHighestHeight = player.GetComponentInParent<TCPLayer>();
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
        GetAndUpdatePlayers();
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

    public List<IPlayer> GetScoreboard()
    {
        GetAndUpdatePlayers();
        var winners = players.OrderByDescending(player => player.transform.position.y).ToList();
        List<IPlayer> playerWinners = new List<IPlayer>();
        foreach (var winner in winners)
        {
            playerWinners.Add(winner.GetComponentInParent<IPlayer>());
        }
        return playerWinners;
    }

    public void SlowDownTopPlayer()
    {
        SlowDownPlayer?.Invoke(this, new SlowDownPlayerArgs
        {
            player = GetHightestHeightPlayer()
        });
    }

    public void GetAndUpdatePlayers()
    {
        List<Transform> newPlayerList = new List<Transform>();
        foreach (var client in NetworkManager.Instance.GetAllPlayers())
        {
            newPlayerList.Add((client as TCPLayer).GetPlayerBody());
        }
        players = newPlayerList;
    }
}
