using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WaitingUI : MonoBehaviour
{
    // Start is called before the first frame update
    void Start()
    {

        TCMiniGameStateManager.Instance.GameStateChanged += Instance_GameStateChanged;
        Show();
    }

    private void Instance_GameStateChanged(object sender, TCMiniGameStateManager.GameStateChangedArgs e)
    {
        if (!TCMiniGameStateManager.Instance.GameIsWaiting())
        {
            Hide();
        }
        else
        {
            Show();
        }
    }

    

    private void Show()
    {
        gameObject.SetActive(true);
    }

    private void Hide()
    {
        gameObject.SetActive(false);
    }
}
