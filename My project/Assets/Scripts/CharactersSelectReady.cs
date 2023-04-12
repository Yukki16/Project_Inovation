using System.Collections;
using System.Collections.Generic;
using Unity.Netcode;
using UnityEngine;
using static TCMiniGameStateManager;

public class CharactersSelectReady : NetworkBehaviour
{
    public static CharactersSelectReady Instance { get; private set; }
    private Dictionary<ulong, bool> playerReadyDictionary;

    [SerializeField] GameObject playerPrefab_1;
    [SerializeField] GameObject playerPrefab_2;
    [SerializeField] GameObject playerPrefab_3;
    [SerializeField] GameObject playerPrefab_4;

    [SerializeField] Transform spawn_1;
    [SerializeField] Transform spawn_2;
    [SerializeField] Transform spawn_3;
    [SerializeField] Transform spawn_4;

    bool pozOneTaken;
    bool pozTwoTaken;
    bool pozThreeTaken;
    bool pozFourTaken;

    Dictionary<ulong, int> playerPosition = new Dictionary<ulong, int>();
    Dictionary<ulong, GameObject> playerGameObject = new Dictionary<ulong, GameObject>();

    public int connections = 0;

    private void Awake()
    {
        Instance = this;
        NetworkManager.Singleton.OnClientConnectedCallback += OnPlayerConnected;
        NetworkManager.Singleton.OnClientDisconnectCallback += OnPlayerDisconnected;
        playerReadyDictionary = new Dictionary<ulong, bool>();
        this.GetComponent<NetworkObject>().Spawn();
    }

    private void OnPlayerConnected(ulong connID)
    {
        connections++;
        if (!pozOneTaken)
        {
            GameObject playerPrefab = Instantiate(playerPrefab_1);
            playerPrefab.transform.position = spawn_1.position;
            playerPrefab.transform.rotation = spawn_1.rotation;
            playerPrefab.GetComponent<NetworkObject>().Spawn();
            pozOneTaken = true;
            playerPosition.Add(connID, 1);
            playerGameObject.Add(connID, playerPrefab);
        }
        else
        if (!pozTwoTaken)
        {
            GameObject playerPrefab = Instantiate(playerPrefab_1);
            playerPrefab.transform.position = spawn_2.position;
            playerPrefab.transform.rotation = spawn_2.rotation;
            playerPrefab.GetComponent<NetworkObject>().Spawn();
            pozTwoTaken = true;
            playerPosition.Add(connID, 2);
            playerGameObject.Add(connID, playerPrefab);
        }
        else
        if (!pozThreeTaken)
        {
            GameObject playerPrefab = Instantiate(playerPrefab_1);
            playerPrefab.transform.position = spawn_3.position;
            playerPrefab.transform.rotation = spawn_3.rotation;
            playerPrefab.GetComponent<NetworkObject>().Spawn();
            pozThreeTaken = true;
            playerPosition.Add(connID, 3);
            playerGameObject.Add(connID, playerPrefab);
        }
        else
        if (!pozFourTaken)
        {
            GameObject playerPrefab = Instantiate(playerPrefab_1);
            playerPrefab.transform.position = spawn_4.position;
            playerPrefab.transform.rotation = spawn_4.rotation;
            playerPrefab.GetComponent<NetworkObject>().Spawn();
            pozFourTaken = true;
            playerPosition.Add(connID, 4);
            playerGameObject.Add(connID, playerPrefab);
        }

        Debug.Log(connections + " " + connID.ToString());
    }

    private void OnPlayerDisconnected(ulong connID)
    {
        connections--;
        if (playerPosition.ContainsKey(connID))
        {
            switch (playerPosition.GetValueOrDefault(connID))
            {
                case 1:
                    pozOneTaken = false;
                    playerGameObject.GetValueOrDefault(connID).GetComponent<NetworkObject>().Despawn();
                    playerGameObject.Remove(connID);
                    break;
                case 2:
                    pozTwoTaken = false;
                    playerGameObject.GetValueOrDefault(connID).GetComponent<NetworkObject>().Despawn();
                    playerGameObject.Remove(connID);
                    break;
                case 3:
                    pozThreeTaken = false;
                    playerGameObject.GetValueOrDefault(connID).GetComponent<NetworkObject>().Despawn();
                    playerGameObject.Remove(connID);
                    break;
                case 4:
                    pozFourTaken = false;
                    playerGameObject.GetValueOrDefault(connID).GetComponent<NetworkObject>().Despawn();
                    playerGameObject.Remove(connID);
                    break;
            }
            playerPosition.Remove(connID);
        }
        else
        {
            Debug.LogWarning("Player that disconnected has not been registered!");
        }
    }
    public void SetPlayerReady()
    {
        if (connections >= 2)
        {
            if (IsServer)
            {
                Loader.LoadNetwork(Loader.Scene.TowerClimb);
            }
            if (IsClient || IsLocalPlayer)
            {
                Loader.LoadNetwork(Loader.Scene.TowerClimb);
            }
        }
    }
}
