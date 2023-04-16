using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Unity.Services.Core;
using Unity.Services.Authentication;
using System;
using Unity.Services.Lobbies;
using Unity.Services.Lobbies.Models;

public class GameLobby : MonoBehaviour
{
    public static GameLobby Instance { get; private set; }

    private void Awake()
    {
        Instance = this;
    }

    public void CreateLobbby()
    {
        try
        {
            Loader.Load(Loader.Scene.Waiting);
        }
        catch (LobbyServiceException ex)
        {
            Debug.LogException(ex);
        }
    }
}
