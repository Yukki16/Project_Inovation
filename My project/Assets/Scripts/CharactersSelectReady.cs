using System.Collections;
using System.Collections.Generic;
using Unity.Netcode;
using UnityEngine;
using static TCMiniGameStateManager;

public class CharactersSelectReady : NetworkBehaviour
{
    public static CharactersSelectReady Instance { get; private set; }
    private Dictionary<ulong, bool> playerReadyDictionary;

    private void Awake()
    {
        Instance = this;
        playerReadyDictionary = new Dictionary<ulong, bool>();
    }

    public void SetPlayerReady()
    {
        if (IsServer)
        {
            Loader.LoadNetwork(Loader.Scene.TowerClimb);
        }
    }
}
