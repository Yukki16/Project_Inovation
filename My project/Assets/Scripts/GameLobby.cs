using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Unity.Services.Core;
using Unity.Services.Authentication;
using System;
using Unity.Services.Lobbies;
using Unity.Netcode;
using Unity.Services.Lobbies.Models;

public class GameLobby : MonoBehaviour
{
    public static GameLobby Instance { get; private set; }

    int indexPlayer = 0;

    [SerializeField] const int maxLobbySize = 4;

    private Lobby joinedLobby;

    private void Awake()
    {
        Instance = this;

        DontDestroyOnLoad(this);

        InitializeUnityAuthentification();
    }

    private async void InitializeUnityAuthentification()
    {
        if (UnityServices.State != ServicesInitializationState.Initialized)
        {
            indexPlayer++;
            InitializationOptions options = new InitializationOptions();
            options.SetProfile("Player" + indexPlayer);

            await UnityServices.InitializeAsync(options);

            await AuthenticationService.Instance.SignInAnonymouslyAsync();
        }
    }

    public async void CreateLobbby()
    {
        try
        {
            await LobbyService.Instance.CreateLobbyAsync("Lobby", maxLobbySize);

            Multiplayer.Instance.StartServer();
            Loader.Load(Loader.Scene.CharacterCreation);
        }
        catch (LobbyServiceException ex)
        {
            Debug.LogException(ex);
        }
    }

    public async void QuickJoin()
    {
        try
        {
            Multiplayer.Instance.StartClient();
            joinedLobby = await LobbyService.Instance.QuickJoinLobbyAsync();
            Loader.Load(Loader.Scene.CharacterCreation);
        }
        catch (LobbyServiceException ex)
        {
            Debug.Log(ex);
        }
    }

}
