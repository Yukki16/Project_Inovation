using System.Collections;
using System.Collections.Generic;
using Unity.Netcode;
using UnityEngine;
using UnityEngine.SceneManagement;

public class PhoneScreenUI : MonoBehaviour
{
    private bool canNotBeShown;
    private void Start()
    {
       
            //Hide();
        TCMiniGameStateManager.Instance.GameStateChanged += Instance_GameStateChanged;

        if(SceneManager.GetActiveScene().name == "TowerClimb")
        {
            Show();
        }
        else
        {
            Hide();
        }
            
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

    /*private void Singleton_OnServerStarted()
    {
        if (IsServer)
        {
            canNotBeShown = false;
        }
        else
        {
            canNotBeShown = true;
        }
    }*/

    public void Show()
    {
        gameObject.SetActive(true);
    }

    public void Hide()
    {
        gameObject.SetActive(false);
    }
}
