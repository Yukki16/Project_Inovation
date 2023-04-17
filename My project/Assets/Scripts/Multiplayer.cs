/*using System;
using System.Collections;
using System.Collections.Generic;
using Unity.Netcode;
using UnityEngine;

public class Multiplayer : NetworkBehaviour
{
    public static Multiplayer Instance { get; private set; }

    private void Awake()
    {
        Instance = this;

        DontDestroyOnLoad(gameObject);
    }

    public void StartServer()
    {
        NetworkManager.Singleton.ConnectionApprovalCallback += NetworkManager_ConnectionApprovalCallback;
        NetworkManager.Singleton.OnClientDisconnectCallback += OnClientDisconnectCallback;
        NetworkManager.Singleton.StartServer();
    }

    private void OnClientDisconnectCallback(ulong obj)
    {
        if (!IsServer)
        {
            Loader.Load(Loader.Scene.MainMenu);
        }  
    }

    private void NetworkManager_ConnectionApprovalCallback(NetworkManager.ConnectionApprovalRequest arg1, NetworkManager.ConnectionApprovalResponse approvalResponse)
    {
        approvalResponse.Approved = true;
        //if (TCMiniGameStateManager.Instance.GameIsWaiting())
        //{
        //    approvalResponse.Approved= true;
        //    approvalResponse.CreatePlayerObject = true;
        //}
        //else
        //{
        //    approvalResponse.Approved = false;
        //    approvalResponse.CreatePlayerObject = false;
        //}
    }

    public void StartClient()
    {
        NetworkManager.Singleton.StartClient();
    }
}
*/