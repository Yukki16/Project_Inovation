using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Unity.Services.Core;
using Unity.Services.Authentication;
using System;

public class Lobby : MonoBehaviour
{
    public static Lobby Instance { get; private set; }

    int indexPlayer = 0;

    [SerializeField] const int maxLobbySize = 4;

    private void Awake()
    {
        Instance = this;

        DontDestroyOnLoad(this);

        InitializeUnityAuthentification();
    }

    private async void InitializeUnityAuthentification()
    {
        if(UnityServices.State != ServicesInitializationState.Initialized)
        {
            indexPlayer++;
            InitializationOptions options = new InitializationOptions();
            options.SetProfile("Player" + indexPlayer);

            await UnityServices.InitializeAsync(options);

            await AuthenticationService.Instance.SignInAnonymouslyAsync();
        }
    }
}
