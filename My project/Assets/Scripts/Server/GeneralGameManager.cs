using Assets.Scripts;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Sockets;
using UnityEngine;
using UnityEngine.SceneManagement;

public class GeneralGameManager : MonoBehaviour
{
    public static GeneralGameManager Instance { get; private set; }
    public enum ServerStates
    {
        IN_LOBBY,
        IN_GAME_SELECTING,
        IN_GAME,
        IN_SCOREBOARD,
        IN_END_SCREEN
    }

    public enum Minigames
    {
        NOT_PLAYABLE,
        TOWER_CLIMB,
        LETSGLIDE
    }

    public enum CharacterColors
    {
        NONE,
        BLACK,
        ORANGE,
        WHITE,
        PURPLE
    }

    private const int MAX_POINTS_TO_EARN = 4;
    private const int MAX_GAME_POINTS = 20;

    private ServerStates currentState;
    private Minigames previousSelectedMinigame = Minigames.NOT_PLAYABLE;
    private Minigames currentSelectedMinigame = Minigames.NOT_PLAYABLE;

    private Dictionary<CharacterColors, TcpClient> clientsWithTheirCharacterColor;
    private Dictionary<TcpClient, int> clientsWithPoints;

    private void Awake()
    {
        Instance = this;
        currentState = ServerStates.IN_LOBBY;
        clientsWithTheirCharacterColor = new Dictionary<CharacterColors, TcpClient>
        {
            { CharacterColors.BLACK, null },
            { CharacterColors.ORANGE, null },
            { CharacterColors.WHITE, null },
            { CharacterColors.PURPLE, null }
        };
        clientsWithPoints = new Dictionary<TcpClient, int>();
        DontDestroyOnLoad(gameObject);
    }

    private void Start()
    {
        SceneManager.sceneLoaded += SceneManager_sceneLoaded;
    }

    private void SceneManager_sceneLoaded(Scene arg0, LoadSceneMode arg1)
    {
        OnSceneLoaded();
    }

    private void OnSceneLoaded()
    {
        if (SceneManager.GetActiveScene().name == "TowerClimb")
        {
            NetworkManager.Instance.AssignMinigameCharacters(Minigames.TOWER_CLIMB);
        }
        else if (SceneManager.GetActiveScene().name == "Gliding")
        {
            NetworkManager.Instance.AssignMinigameCharacters(Minigames.LETSGLIDE);
        }
    }

    public string AddClient(TcpClient client)
    {
        clientsWithPoints.Add(client, 0);
        CharacterColors colorToUse = CharacterColors.NONE;
        foreach (var color in clientsWithTheirCharacterColor.Keys)
        {
            if (clientsWithTheirCharacterColor[color] == null)
            {
                colorToUse = color;
                break;
            }
        }

        if (colorToUse != CharacterColors.NONE)
        {
            clientsWithTheirCharacterColor[colorToUse] = client;
            if (colorToUse == CharacterColors.BLACK)
            {
                return "MIKKI";
            }
            else if (colorToUse == CharacterColors.ORANGE)
            {
                return "JERR";
            }
            else if (colorToUse == CharacterColors.WHITE)
            {
                return "SALVI";
            }
            else if (colorToUse == CharacterColors.PURPLE)
            {
                return "MEOW";
            }
        }
        return null;
    }

    public void RemoveClient(TcpClient client)
    {
        clientsWithPoints.Remove(client);
        CharacterColors colorToUse = CharacterColors.NONE;
        foreach (var color in clientsWithTheirCharacterColor.Keys)
        {
            if (clientsWithTheirCharacterColor[color] == client)
            {
                colorToUse = color;
                break;
            }
        }

        if (colorToUse != CharacterColors.NONE)
        {
            clientsWithTheirCharacterColor[colorToUse] = null;
        }
    }

    public Dictionary<CharacterColors, TcpClient> GetClientsWithTheirCharacterColor()
    {
        return clientsWithTheirCharacterColor;
    }

    public ServerStates GetCurrentServerState()
    {
        return currentState;
    }

    public void HandleServerDisconnect()
    {
        Destroy(gameObject);
    }

    private void SelectMinigame()
    {
        List<Minigames> allMinigames = Enum.GetValues(typeof(Minigames)).Cast<Minigames>().ToList();
        bool gameSelected = false;
        do
        {
            Minigames newMinigame = allMinigames[UnityEngine.Random.Range(1, allMinigames.Count)];
            if (previousSelectedMinigame == Minigames.NOT_PLAYABLE || newMinigame != previousSelectedMinigame)
            {
                previousSelectedMinigame = currentSelectedMinigame;
                //currentSelectedMinigame = newMinigame;
                currentSelectedMinigame = Minigames.LETSGLIDE;
                currentState = ServerStates.IN_GAME_SELECTING;
                gameSelected = true;
            }
        } while (!gameSelected);      
    }

    public Minigames GetCurrentChosenMinigame()
    {
        return currentSelectedMinigame;
    }

    public void StartGame()
    {
        PerformGameSelecting();
    }

    public void EndMinigame()
    {
        if (GetCurrentChosenMinigame() == Minigames.TOWER_CLIMB)
        {
            List<IPlayer> scoreboard = GameManager.Instance.GetScoreboard();
            Dictionary<TcpClient, IPlayer> clientsWithAssociatedPlayers = NetworkManager.Instance.GetClientWithAssociatedPlayers();
            for (int i = 0; i < scoreboard.Count; i++)
            {
                foreach (var client in clientsWithAssociatedPlayers.Keys)
                {
                    if (clientsWithAssociatedPlayers[client] == scoreboard[i])
                    {
                        clientsWithPoints[client] += MAX_POINTS_TO_EARN - i;
                        break;
                    }
                }
            }
        }
        else if (GetCurrentChosenMinigame() == Minigames.LETSGLIDE)
        {
            List<IPlayer> scoreboard = GlidingGameManager.Instance.GetScoreboard();
            Dictionary<TcpClient, IPlayer> clientsWithAssociatedPlayers = NetworkManager.Instance.GetClientWithAssociatedPlayers();
            for (int i = 0; i < scoreboard.Count; i++)
            {
                foreach (var client in clientsWithAssociatedPlayers.Keys)
                {
                    if (clientsWithAssociatedPlayers[client] == scoreboard[i])
                    {
                        clientsWithPoints[client] += MAX_POINTS_TO_EARN - i;
                        break;
                    }
                }
            }
        }

        currentSelectedMinigame = Minigames.NOT_PLAYABLE;
        currentState = ServerStates.IN_SCOREBOARD;
        NetworkManager.Instance.NotifyClientsForScoreboard();
        Loader.Load(Loader.Scene.Scoreboard);    
    }

    public void LoadMinigame()
    {
        currentState = ServerStates.IN_GAME;
        
        switch (currentSelectedMinigame)
        {
            case Minigames.TOWER_CLIMB:
                NetworkManager.Instance.LoadNetwork(Loader.Scene.TowerClimb);
                break;
            case Minigames.LETSGLIDE:
                NetworkManager.Instance.LoadNetwork(Loader.Scene.Gliding);
                break;
        }
    }

    public void CheckIfOtherMinigameShouldBeStarted()
    {
        if (MaxPointsReached())
        {
            PerformGameEnd();
        }
        else
        {
            PerformGameSelecting();
        }
    }

    private bool MaxPointsReached()
    {
        foreach (var point in clientsWithPoints.Values)
        {
            if (point >= MAX_GAME_POINTS)
            {
                return true;
            }
        }
        return false;
    }

    public TcpClient GetWinner()
    {
        return clientsWithPoints.OrderByDescending(key => key.Value).FirstOrDefault().Key;
    }

    private void PerformGameSelecting()
    {
        SelectMinigame();
        NetworkManager.Instance.LoadNetwork(Loader.Scene.GameSelecting);
    }

    private void PerformGameEnd()
    {
        currentState = ServerStates.IN_END_SCREEN;
        NetworkManager.Instance.LoadNetwork(Loader.Scene.Ending);
        NetworkManager.Instance.NotifyClientsForGameEnd();
    }

    public Dictionary<TcpClient, int> ReturnClientsPoints()
    {
        return clientsWithPoints;
    }
}
