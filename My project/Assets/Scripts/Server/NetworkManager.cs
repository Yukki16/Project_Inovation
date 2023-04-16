using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Sockets;
using System.Text;
using System.Threading;
using UnityEditor;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.TextCore.Text;
using static GeneralGameManager;

public class NetworkManager : MonoBehaviour
{
    public static NetworkManager Instance { get; private set; }

    public event EventHandler OnAllPlayersJoined;

    [SerializeField] private Transform playerPrefab;

    private const int MIN_NETWORK_CLIENTS = 2;
    private const int MAX_NETWORK_CLIENTS = 4;
    
    private readonly static List<string> possibleActions = new List<string> { "/moveright", "/moveleft", "/leave" };
    private static Dictionary<TcpClient, TCPLayer> _clients = new Dictionary<TcpClient, TCPLayer>();

    private TcpListener _listener;
    

    private void Awake()
    {
        Instance = this;
        
        DontDestroyOnLoad(this);
    }

    private void Start()
    {
        _listener = new TcpListener(IPAddress.Any, 7777);
        _listener.Start();
        Debug.Log("Server started on port 7777");

    }


    private void Update()
    {
        try
        {
            CheckIfAllClientsAreConnected();
            ProcessNewClients();

            //if (GeneralGameManager.Instance.GetCurrentServerState() != GeneralGameManager.ServerStates.IN_GAME)
            //{
            //    ProcessNewClients();
            //}
            //else
            //{
            //    ProcessGame();
            //}
            
        }
        catch (Exception e)
        {
            Debug.LogError("Error occured:");
            Debug.LogError(e.Message);
        }
    }

    private void FixedUpdate()
    {
        try
        {
            ProcessExistingClients();
        }
        catch (Exception e)
        {
            Debug.LogError("Error occured:");
            Debug.LogError(e.Message);
        }
    }

    private void ProcessGame()
    {


    }

    private void CheckIfAllClientsAreConnected()
    {
        try
        {
            if (_clients.Count > 0)
            {
                Dictionary<TcpClient, TCPLayer> clients2 = CreateDirectoryCopy(_clients);
                foreach (TcpClient client in clients2.Keys)
                {
                    try
                    {
                        /*If this is true, the client is not responding and not active anymore
                          This prevents the bug that occurs when clients disconnect out of nowhere*/
                        if (client.Client.Poll(1, SelectMode.SelectRead) && client.Client.Available == 0)
                        {
                            CharacterWaiting.Instance.RemoveCharacter(client);
                            client.Close();
                            _clients.Remove(client);

                        }
                    }
                    catch (SocketException)
                    {
                        CharacterWaiting.Instance.RemoveCharacter(client);
                        client.Close();
                        _clients.Remove(client);

                    }
                }
            }         
        }
        catch (Exception)
        {
            throw;
        }
    }

    private void ProcessNewClients()
    {
        try
        {
            while (_listener.Pending())
            {
                if (_clients.Count < MAX_NETWORK_CLIENTS)
                {
                    TcpClient client = _listener.AcceptTcpClient();
                    _clients.Add(client, null);
                    CharacterWaiting.Instance.AddCharacter(client);
                    Debug.Log("Accepted new client.");
                }
                else
                {
                    Debug.Log("Refused client.");
                    _listener.AcceptTcpClient().Close();
                }
            }
        }
        catch (Exception)
        {
            throw;
        }
    }

    private void ProcessExistingClients()
    {
        try
        {
            Dictionary<TcpClient, TCPLayer> clients2 = CreateDirectoryCopy(_clients);
            foreach (KeyValuePair<TcpClient, TCPLayer> client in clients2)
            {
                if (GeneralGameManager.Instance.GetCurrentServerState() == ServerStates.IN_GAME)
                {
                    if (client.Key.Available == 0) continue;

                    string fullMessage = ReadCustomMessage(client);

                    CheckAndExecuteCommand(fullMessage, client);
                }
                
            }
        }
        catch (Exception)
        {
            throw;
        }
    }

    private static bool CheckAndExecuteCommand(string fullMessage, KeyValuePair<TcpClient, TCPLayer> client)
    {
        fullMessage = fullMessage.ToLower();
        if (fullMessage.StartsWith("/"))
        {
            if (possibleActions.Contains(fullMessage))
            {
                ExecuteCommands(fullMessage, client);
                return true;
            }
        }
        return false;
    }

    private static void ExecuteCommands(string fullMessage, KeyValuePair<TcpClient, TCPLayer> client)
    {
        Debug.Log(fullMessage);
        switch (fullMessage)
        {
            case "/moveleft":
                handleMovement(client.Key, Vector2.left);
                break;
            case "/moveright":
                handleMovement(client.Key, Vector2.right);
                break;
            case "/leave":
                //leave
                break;
        }
    }

    private static void SendCustomMessage(TcpClient client, string message)
    {
        try
        {
            NetworkStream stream = client.GetStream();
            byte[] outBytes = Encoding.UTF8.GetBytes(message);
            StreamUtil.Write(stream, outBytes);
        }
        catch (Exception)
        {
            throw;
        }
    }

    private static string ReadCustomMessage(KeyValuePair<TcpClient, TCPLayer> client)
    {
        NetworkStream stream = client.Key.GetStream();
        byte[] readStream = StreamUtil.Read(stream);
        return Encoding.UTF8.GetString(readStream);
    }

    private static void handleDisconnects(TcpClient client)
    {
        _clients.Remove(client);
    }

    private static void handleMovement(TcpClient clientToMove, Vector2 moveDir)
    {
        _clients[clientToMove].HandleSideMovement(moveDir);
    }

    private static Dictionary<TcpClient, TCPLayer> CreateDirectoryCopy(Dictionary<TcpClient, TCPLayer> directoryToCopy)
    {
        try
        {
            Dictionary<TcpClient, TCPLayer> directory2 = new Dictionary<TcpClient, TCPLayer>();
            foreach (KeyValuePair<TcpClient, TCPLayer> keyvalue in directoryToCopy)
            {
                directory2.Add(keyvalue.Key, keyvalue.Value);
            }
            return directory2;
        }
        catch (Exception)
        {
            throw;
        }
    }

    public void DisconnectServer()
    {
        _clients.Clear();
        _listener.Stop();
        Destroy(gameObject);
        GeneralGameManager.Instance.HandleServerDisconnect();
    }

    public void LoadNetwork(Loader.Scene networkScene)
    {
        Loader.Load(networkScene);
        SendToAllClients($"/loadscene:{networkScene}");
    }

    public void SendToAllClients(string message)
    {
        foreach (var client in _clients.Keys)
        {
            SendCustomMessage(client, message);
        }
    }

    public void ServerIsReady()
    {
        if (GetAmountOfConnectedPlayers() >= MIN_NETWORK_CLIENTS)
        {
            GeneralGameManager.Instance.StartGame();
        }
    }

    public int GetAmountOfConnectedPlayers()
    {
        return _clients.Count;
    }

    public void AssignMinigameCharacters(GeneralGameManager.Minigames currentMinigame)
    {
        switch (currentMinigame)
        {
            case Minigames.TOWER_CLIMB:
                if (SceneManager.GetActiveScene().name == "TowerClimb")
                {
                    List<TCPLayer> allPlayers = GameObject.FindObjectsOfType<TCPLayer>().ToList();
                    Dictionary<TcpClient, TCPLayer> clients2 = CreateDirectoryCopy(_clients);
                    foreach (var client in clients2.Keys)
                    {
                        _clients[client] = allPlayers[0];
                        allPlayers.RemoveAt(0);
                    }

                    foreach (var player in allPlayers)
                    {
                        Destroy(player.gameObject);
                    }
                    OnAllPlayersJoined?.Invoke(this, EventArgs.Empty);
                    SendToAllClients("/loadgame:towerclimb");
                }
                else
                {
                    Debug.LogError(SceneManager.GetActiveScene().ToString());
                    Debug.LogError("Method was called before towerclimb scene was loaded.");
                }
                
                break;
            case Minigames.LETSGLIDE:
                //TODO
                break;
        }
    }

    public void NotifyClientsForGameStart()
    {
        SendToAllClients("/startgame");
    }

    public List<TCPLayer> GetTowerClimbPlayers()
    {
        return _clients.Values.ToList();
    }
}
