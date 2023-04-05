using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Mirror;

public class GameManager : NetworkBehaviour
{
    public static GameManager Instance { get; private set; }

    CustomNetwork networkVar;
    [SerializeField] private Transform[] players;
    [SerializeField] private ObjectSpawner objectSpawner;

    private const float MAXGAMELENGTH = 900;
    private const float MAXGAMETIME = 120;

    //Spawn
    private const float MINSPAWNRATE = 0.6f;
    private const float MAXSPAWNMODIFIERTIME = 5;
    private const float SPAWNRATEDECREASE = 0.2f;
    private float spawnModifierTimer;
    private float maxSpawnRate = 1.5f;
    private float spawnRate = 1.5f;

    private void Awake()
    {
        Instance = this;
        spawnModifierTimer = MAXSPAWNMODIFIERTIME;
        spawnRate = maxSpawnRate;

        networkVar = GameObject.FindGameObjectWithTag("NetworkManager").GetComponent<CustomNetwork>();
    }

    public float GetLowestHeightOfAllPlayers()
    {
        float lowestHeight = networkVar.ReturnCurrentPlayers()[0].gameObject.transform.position.y;
        foreach (GameObject player in networkVar.ReturnCurrentPlayers())
        {
            if (player.transform.position.y <= lowestHeight) { 
                lowestHeight = player.transform.position.y;
            }
        }
        return lowestHeight;
    }

    public float GetHighestHeightOfAllPlayers()
    {
        float highestHeight = networkVar.ReturnCurrentPlayers()[0].gameObject.transform.position.y;
        foreach (GameObject player in networkVar.ReturnCurrentPlayers())
        {
            if (player.transform.position.y >= highestHeight)
            {
                highestHeight = player.transform.position.y;
            }
        }
        return highestHeight;
    }

    public string GetNicknameOfHightestHightPlayer()
    {
        
        float highestHeight = networkVar.ReturnCurrentPlayers()[0].gameObject.transform.position.y;
        string playerNicknameWithHighestHeight = networkVar.ReturnCurrentPlayers()[0].gameObject.transform.GetComponentInParent<Player>().GetNickname();
        foreach (GameObject player in networkVar.ReturnCurrentPlayers())
        {
            if (player.transform.position.y >= highestHeight)
            {
                highestHeight = player.transform.position.y;
                playerNicknameWithHighestHeight = player.GetComponentInParent<Player>().GetNickname();
            }
        }
        return playerNicknameWithHighestHeight;
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
            if (GetHighestHeightOfAllPlayers() >= GetMaxGameLength())
            {
                TCMiniGameStateManager.Instance.EndGame();
                Debug.Log(GetNicknameOfHightestHightPlayer());
                Debug.Log(GetScoreboard());
            }
        }
    }

    private void SpawnFallingItems()
    {
        foreach (GameObject player in networkVar.ReturnCurrentPlayers())
        {
            Debug.Log(player.transform.Find("PlayerBody").transform.position);
            objectSpawner.SpawnObject(player.transform.Find("PlayerBody").transform.position);
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

    public Dictionary<string,int> GetScoreboard()
    {
        Dictionary<string, int> scoreboardResults = new Dictionary<string,int>();
        foreach (GameObject player in networkVar.ReturnCurrentPlayers())
        {
            Player currentPlayer = player.GetComponentInParent<Player>();
            scoreboardResults.Add(currentPlayer.GetNickname(), currentPlayer.GetScore());
        }
        return scoreboardResults;
    }
}
