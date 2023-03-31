using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameManager : MonoBehaviour
{
    public static GameManager Instance { get; private set; }

    [SerializeField] private Transform[] players;
    [SerializeField] private ObjectSpawner objectSpawner;

    private const float MAXGAMELENGTH = 500;

    //Spawn
    private const float MINSPAWNRATE = 0.6f;
    private const float MAXSPAWNMODIFIERTIME = 5;
    private const float SPAWNRATEDECREASE = 0.2f;
    private float spawnModifierTimer;
    private float maxSpawnRate = 2;
    private float spawnRate = 2;

    private void Awake()
    {
        Instance = this;
        spawnModifierTimer = MAXSPAWNMODIFIERTIME;
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

    public float GetMaxGameLength()
    {
        return MAXGAMELENGTH;
    }

    private void Update()
    {
        if (TCMiniGameManager.Instance.GameIsPlaying())
        {
            HandleSpawningOfItems();
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
}
