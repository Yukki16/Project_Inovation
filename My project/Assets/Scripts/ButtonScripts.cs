using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using TMPro;
using UnityEngine.UI;
using Unity.Services.Lobbies.Models;

public class ButtonScripts : MonoBehaviour
{
    [SerializeField] Button mainMenuButton;
    [SerializeField] Button createLobbyButton;
    [SerializeField] Button quickJoinButton;
    [SerializeField] Button joinCodeButton;

    [SerializeField] TMP_InputField joinCodeInputField;
    [SerializeField] TMP_InputField playerNameInputField;

    [SerializeField] GameLobby lobbyCreateUI;

    [SerializeField] Transform lobbyContainer;
    [SerializeField] Transform lobbyTemplate;

    [SerializeField] GameObject normalUI;
    [SerializeField] GameObject createLobbyUI;

    [SerializeField] Button closeButton;
    [SerializeField] Button createPublicButton;
    [SerializeField] Button createPrivateButton;
    [SerializeField] TMP_InputField lobbyNameInputField;

    private void Awake()
    {
        mainMenuButton.onClick.AddListener(() => {
            GameLobby.Instance.LeaveLobby();
            Loader.Load(Loader.Scene.MainMenu);
        });
        createLobbyButton.onClick.AddListener(() => {
            normalUI.SetActive(false);
            createLobbyUI.SetActive(true);
        });
        quickJoinButton.onClick.AddListener(() => {
            GameLobby.Instance.QuickJoin();
        });
        joinCodeButton.onClick.AddListener(() => {
            GameLobby.Instance.JoinWithCode(joinCodeInputField.text);
        });
        createPublicButton.onClick.AddListener(() => {
            GameLobby.Instance.CreateLobby(lobbyNameInputField.text, false);
        });
        createPrivateButton.onClick.AddListener(() => {
            GameLobby.Instance.CreateLobby(lobbyNameInputField.text, true);
        });
        closeButton.onClick.AddListener(() => {
            normalUI.SetActive(true);
            createLobbyUI.SetActive(false);
        });

        lobbyTemplate.gameObject.SetActive(false);
    }

    private void Start()
    {
        GameLobby.Instance.OnLobbyListChanged += GameLobby_OnLobbyListChanged;
        UpdateLobbyList(new List<Lobby>());
    }

    private void GameLobby_OnLobbyListChanged(object sender, GameLobby.OnLobbyListChangedEventArgs e)
    {
        UpdateLobbyList(e.lobbyList);
    }

    private void UpdateLobbyList(List<Lobby> lobbyList)
    {
        foreach (Transform child in lobbyContainer)
        {
            if (child == lobbyTemplate) continue;
            Destroy(child.gameObject);
        }

        foreach (Lobby lobby in lobbyList)
        {
            Transform lobbyTransform = Instantiate(lobbyTemplate, lobbyContainer);
            lobbyTransform.gameObject.SetActive(true);
            lobbyTransform.GetComponent<LobbyListSingleUI>().SetLobby(lobby);
        }
    }
    public void LoadScene(string scene)
    {
        if (Enum.TryParse(scene, out Loader.Scene newScene))
        {
            Loader.Load(newScene);
        }
        else
        {
            Debug.LogError("Scene couldn't be loaded because Parse failed.");
        }
    }

    
}
