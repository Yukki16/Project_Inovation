using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using Mirror;

public class ButtonScript : MonoBehaviour
{
    [SerializeField] GameObject selectionMenu;
    [SerializeField] GameObject playMenu;


    public void Play()
    {
        selectionMenu.SetActive(false);
        playMenu.SetActive(true);
    }

    public void BackToSelection()
    {
        selectionMenu.SetActive(true);
        playMenu.SetActive(false);
    }

    public void Quit()
    {
        Debug.Log("Application closed");
        Application.Quit();
    }

    public void JoinLobby()
    {
        if(NetworkServer.active)
        {
            Debug.Log("Server found and client started");
            NetworkManager.singleton.StartClient();
        }
    }
}
