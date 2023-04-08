using System.Collections;
using System.Collections.Generic;
using Unity.Netcode;
using UnityEngine;

public class PhoneScreenUI : NetworkBehaviour
{
    private bool canNotBeShown;
    private void Start()
    {
        Hide();
        TCMiniGameStateManager.Instance.GameStateChanged += Instance_GameStateChanged;
        NetworkManager.Singleton.OnServerStarted += Singleton_OnServerStarted;
    }

    private void Instance_GameStateChanged(object sender, TCMiniGameStateManager.GameStateChangedArgs e)
    {
        if (e.gameState == TCMiniGameStateManager.GameState.PLAYING)
        {     
            if (!canNotBeShown)
            {
                Show();
            }
            else
            {
                Hide();
            }
        }
    }

    private void Singleton_OnServerStarted()
    {
        if (IsServer)
        {
            canNotBeShown = false;
        }
        else
        {
            canNotBeShown = true;
        }
    }

    public void Show()
    {
        gameObject.SetActive(true);
    }

    public void Hide()
    {
        gameObject.SetActive(false);
    }
}
