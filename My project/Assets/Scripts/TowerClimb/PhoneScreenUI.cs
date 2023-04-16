using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PhoneScreenUI : MonoBehaviour
{
    private bool canNotBeShown;
    private void Awake()
    {
        //Hide();
        TCMiniGameStateManager.Instance.GameStateChanged += Instance_GameStateChanged;
        //NetworkManager.Singleton.OnServerStarted += Singleton_OnServerStarted;
    }

    private void Instance_GameStateChanged(object sender, TCMiniGameStateManager.GameStateChangedArgs e)
    {
        if (e.gameState == TCMiniGameStateManager.GameState.IN_COUNTDOWN || e.gameState == TCMiniGameStateManager.GameState.PLAYING)
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
