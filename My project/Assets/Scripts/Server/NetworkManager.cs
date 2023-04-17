using Assets.Scripts;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Sockets;
using System.Text;
using System.Threading;
using UnityEditor;
using UnityEditor.PackageManager;
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
    private static Dictionary<TcpClient, IPlayer> _clients = new Dictionary<TcpClient, IPlayer>();

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

    private void FixedUpdate()
    {
        try
        {
            CheckIfAllClientsAreConnected();
            ProcessNewClients();
            ProcessExistingClients();
        }
        catch (Exception e)
        {
            Debug.LogError("Error occured:");
            Debug.LogError(e.Message);
        }
    }

    private void CheckIfAllClientsAreConnected()
    {
        try
        {
            if (_clients.Count > 0)
            {
                Dictionary<TcpClient, IPlayer> clients2 = CreateDirectoryCopy(_clients);
                foreach (TcpClient client in clients2.Keys)
                {
                    try
                    {
                        /*If this is true, the client is not responding and not active anymore
                          This prevents the bug that occurs when clients disconnect out of nowhere*/
                        if (client.Client.Poll(1, SelectMode.SelectRead) && client.Client.Available == 0)
                        {
                            GeneralGameManager.Instance.RemoveClient(client);
                            
                            if (GeneralGameManager.Instance.GetCurrentServerState() == GeneralGameManager.ServerStates.IN_GAME)
                            {
                                if (GeneralGameManager.Instance.GetCurrentChosenMinigame() == Minigames.TOWER_CLIMB)
                                {
                                    TCPLayer playerToRemove = (_clients[client] as TCPLayer);
                                    
                                    if(ProgressBar.Instance != null)
                                    {
                                        ProgressBar.Instance.UpdateProgressBarOnDisconect();
                                    }
                                    _clients.Remove(client);
                                    CameraManager.Instance.UpdateScreenView(GetAmountOfConnectedPlayers());
                                    Destroy(playerToRemove.gameObject);
                                }
                            }
                            else
                            {
                                _clients.Remove(client);
                            }
                            
                            client.Close();
                            
                            
                            if (GeneralGameManager.Instance.GetCurrentServerState() == GeneralGameManager.ServerStates.IN_LOBBY)
                            {
                                CharacterWaiting.Instance.UpdateCharacterWaitingView();
                            }           
                        }
                    }
                    catch (SocketException)
                    {
                        GeneralGameManager.Instance.RemoveClient(client);
                        _clients.Remove(client);
                        if (GeneralGameManager.Instance.GetCurrentServerState() == GeneralGameManager.ServerStates.IN_GAME)
                        {
                            if (GeneralGameManager.Instance.GetCurrentChosenMinigame() == Minigames.TOWER_CLIMB)
                            {
                                CameraManager.Instance.UpdateScreenView(GetAmountOfConnectedPlayers());
                                Destroy((_clients[client] as TCPLayer).gameObject);
                            }
                        }
                        client.Close();


                        if (GeneralGameManager.Instance.GetCurrentServerState() == GeneralGameManager.ServerStates.IN_LOBBY)
                        {
                            CharacterWaiting.Instance.UpdateCharacterWaitingView();
                        }

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
                if (_clients.Count < MAX_NETWORK_CLIENTS && GeneralGameManager.Instance.GetCurrentServerState() == GeneralGameManager.ServerStates.IN_LOBBY)
                {
                    TcpClient client = _listener.AcceptTcpClient();
                    _clients.Add(client, null);
                    GeneralGameManager.Instance.AddClient(client);
                    CharacterWaiting.Instance.UpdateCharacterWaitingView();
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
            Dictionary<TcpClient, IPlayer> clients2 = CreateDirectoryCopy(_clients);
            foreach (KeyValuePair<TcpClient, IPlayer> client in clients2)
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

    private static bool CheckAndExecuteCommand(string fullMessage, KeyValuePair<TcpClient, IPlayer> client)
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

    private static void ExecuteCommands(string fullMessage, KeyValuePair<TcpClient, IPlayer> client)
    {
        Debug.Log(fullMessage);
        switch (fullMessage)
        {
            case "/moveleft":
                if (GeneralGameManager.Instance.GetCurrentServerState() == ServerStates.IN_GAME)
                {
                    if (GeneralGameManager.Instance.GetCurrentChosenMinigame() == Minigames.TOWER_CLIMB)
                    {
                        if (TCMiniGameStateManager.Instance.GameIsPlaying())
                        {
                            handleMovement(client.Key, Vector2.left);
                        }
                    }                    
                }           
                break;
            case "/moveright":
                if (GeneralGameManager.Instance.GetCurrentServerState() == ServerStates.IN_GAME)
                {
                    if (GeneralGameManager.Instance.GetCurrentChosenMinigame() == Minigames.TOWER_CLIMB)
                    {
                        if (TCMiniGameStateManager.Instance.GameIsPlaying())
                        {
                            handleMovement(client.Key, Vector2.right);
                        }
                    }
                }
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

    private static string ReadCustomMessage(KeyValuePair<TcpClient, IPlayer> client)
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
        if (GeneralGameManager.Instance.GetCurrentChosenMinigame() == Minigames.TOWER_CLIMB)
        {
            (_clients[clientToMove] as TCPLayer).HandleSideMovement(moveDir);
        }
        
    }

    private static Dictionary<TcpClient, IPlayer> CreateDirectoryCopy(Dictionary<TcpClient, IPlayer> directoryToCopy)
    {
        try
        {
            Dictionary<TcpClient, IPlayer> directory2 = new Dictionary<TcpClient, IPlayer>();
            foreach (KeyValuePair<TcpClient, IPlayer> keyvalue in directoryToCopy)
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
                    Dictionary<CharacterColors,TcpClient> clientsWithColor = GeneralGameManager.Instance.GetClientsWithTheirCharacterColor();
                    Dictionary<TcpClient, IPlayer> clients2 = CreateDirectoryCopy(_clients);
                    foreach (var color in clientsWithColor.Keys)
                    {
                        if (clientsWithColor[color] != null)
                        {
                            foreach (var player in allPlayers)
                            {
                                if (color == player.GetCharacterColor())
                                {
                                    _clients[clientsWithColor[color]] = player;
                                    allPlayers.Remove(player);
                                    break;
                                }
                            }
                        }
                        
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

    public List<IPlayer> GetAllPlayers()
    {
        return _clients.Values.ToList();
    }

    public Dictionary<TcpClient,IPlayer> GetClientWithAssociatedPlayers()
    {
        return _clients;
    }
}
