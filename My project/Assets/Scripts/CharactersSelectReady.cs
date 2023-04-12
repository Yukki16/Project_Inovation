using System.Collections;
using System.Collections.Generic;
using Unity.Netcode;
using UnityEngine;
using static TCMiniGameStateManager;

public class CharactersSelectReady : NetworkBehaviour
{
    public static CharactersSelectReady Instance { get; private set; }
    private Dictionary<ulong, bool> playerReadyDictionary;

    public int connections = 0;

    private void Awake()
    {
        Instance = this;
        NetworkManager.Singleton.OnClientConnectedCallback += OnPlayerConnected;
        playerReadyDictionary = new Dictionary<ulong, bool>();
        this.GetComponent<NetworkObject>().Spawn();
    }

    private void OnPlayerConnected(ulong connID)
    {
        connections++;
        Debug.Log(connections + " " + connID.ToString());
    }
    public void SetPlayerReady()
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
