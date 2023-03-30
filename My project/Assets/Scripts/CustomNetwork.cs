using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Mirror;
using UnityEngine.SceneManagement;
using UnityEngine.Events;
using System;

public class CustomNetwork : NetworkManager
{
    
    [SerializeField] Transform startPosition_1;
    [SerializeField] Transform startPosition_2;
    [SerializeField] Transform startPosition_3;
    [SerializeField] Transform startPosition_4;

    [SerializeField] GameObject playerPrefab_1;
    [SerializeField] GameObject playerPrefab_2;
    [SerializeField] GameObject playerPrefab_3;
    [SerializeField] GameObject playerPrefab_4;

    [SerializeField] GlidingInput gliding;

    List<GameObject> players = new List<GameObject>();

    public override void OnStartServer()
    {
        base.OnStartServer();
        //gliding.SetupServer();
    }

    private void FixedUpdate()
    {
       
    }
    private void OnEnable()
    {
        SceneManager.sceneLoaded += NewSceneLoaded;
    }

    private void NewSceneLoaded(Scene arg0, LoadSceneMode arg1)
    {
        try
        {
            startPosition_1 = GameObject.Find("Spawn1").transform;
            startPosition_2 = GameObject.Find("Spawn2").transform;
            startPosition_3 = GameObject.Find("Spawn3").transform;
            startPosition_4 = GameObject.Find("Spawn4").transform;
        }
        catch
        {
            if(!startPosition_1 || !startPosition_2 || !startPosition_3|| !startPosition_4)
            {
                Debug.LogWarning("Position not found");
            }
        }
    }

    public override void OnClientDisconnect()
    {
        for (int i = players.Count; i > 0; i--)
        {
            if(players[i] == null)
                players.RemoveAt(i);
        }
    }


    public override void OnServerAddPlayer(NetworkConnectionToClient conn)
    {
        Debug.Log("Connection in making");

        GameObject player = null;
        switch (numPlayers)
        {
            case 0:
                player = startPosition_1 != null
            ? Instantiate(playerPrefab_1, startPosition_1.position, startPosition_1.rotation)
            : Instantiate(playerPrefab_1);
                break;
            case 1:
                player = startPosition_2 != null
            ? Instantiate(playerPrefab_2, startPosition_2.position, startPosition_2.rotation)
            : Instantiate(playerPrefab_2);
                break;
            case 2:
                player = startPosition_3 != null
            ? Instantiate(playerPrefab_3, startPosition_3.position, startPosition_3.rotation)
            : Instantiate(playerPrefab_3);
                break;
            case 3:
                player = startPosition_4 != null
            ? Instantiate(playerPrefab_4, startPosition_4.position, startPosition_4.rotation)
            : Instantiate(playerPrefab_4);
                break;
        }
        if (player != null)
        {
            Debug.Log("Adding Player");
            player.name = $"{playerPrefab.name} [connId={conn.connectionId}]";
            players.Add(player);
            NetworkServer.AddPlayerForConnection(conn, player);
            AddCameras();
        }
        else
        {
            Debug.LogWarning("Player is empty");
        }
    }

    void AddCameras()
    {
        GameObject camObject = null;
        Camera cam = null;

        switch (numPlayers)
        {
            default: Debug.LogWarning("No players found");
                break;
            case 1:
                camObject = new GameObject();
                camObject.name = "Camera_" + players[0].name;
                cam = camObject.AddComponent<Camera>();
                camObject.AddComponent<FollowPlayer>().target = players[0].transform;
                //camObject.transform.parent = players[0].transform;
                cam.rect = new Rect(0, 0, 1, 1);
                break;
            case 2:
                camObject = new GameObject();
                camObject.name = "Camera_" + players[1].name;
                cam = camObject.AddComponent<Camera>();
                camObject.AddComponent<FollowPlayer>().target = players[0].transform;
                cam.rect = new Rect(0.5f, 0, 0.5f, 1);

                cam = GameObject.Find("Camera_" + players[0].name).GetComponent<Camera>();
                cam.rect = new Rect(0, 0, 0.5f, 1);
                break;
            case 3:
                camObject = new GameObject();
                camObject.name = "Camera_" + players[2].name;
                cam = camObject.AddComponent<Camera>();
                camObject.AddComponent<FollowPlayer>().target = players[0].transform;
                cam.rect = new Rect(0.25f, 0.5f, 0.5f, 0.5f);

                cam = GameObject.Find("Camera_" + players[0].name).GetComponent<Camera>();
                cam.rect = new Rect(0, 0, 0.5f, 0.5f);

                cam = GameObject.Find("Camera_" + players[1].name).GetComponent<Camera>();
                cam.rect = new Rect(0.5f, 0, 0.5f, 0.5f);
                break;
            case 4:
                camObject = new GameObject();
                camObject.name = "Camera_" + players[3].name;
                cam = camObject.AddComponent<Camera>();
                camObject.AddComponent<FollowPlayer>().target = players[0].transform;
                cam.rect = new Rect(0.5f, 0.5f, 0.5f, 0.5f);

                cam = GameObject.Find("Camera_" + players[0].name).GetComponent<Camera>();
                cam.rect = new Rect(0, 0, 0.5f, 0.5f);

                cam = GameObject.Find("Camera_" + players[1].name).GetComponent<Camera>();
                cam.rect = new Rect(0.5f, 0, 0.5f, 0.5f);

                cam = GameObject.Find("Camera_" + players[2].name).GetComponent<Camera>();
                cam.rect = new Rect(0, 0.5f, 0.5f, 0.5f);
                break;
        }
    }
}
