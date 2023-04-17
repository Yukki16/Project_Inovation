using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class CountdownUI : MonoBehaviour
{
    [SerializeField] private TextMeshProUGUI text;

    private void Start()
    {
        HideCountdown();
        if (GeneralGameManager.Instance.GetCurrentChosenMinigame() == GeneralGameManager.Minigames.TOWER_CLIMB)
        {
            TCMiniGameStateManager.Instance.GameStateChanged += Instance_GameStateChanged;
        }
        else if (GeneralGameManager.Instance.GetCurrentChosenMinigame() == GeneralGameManager.Minigames.LETSGLIDE)
        {
            GlidingGameManager.Instance.GameStateChanged += Instance_GameStateChanged1;
        }
        
        
    }

    private void Instance_GameStateChanged1(object sender, GlidingGameManager.GameStateChangedArgs e)
    {
        if (e.gameState != GlidingGameManager.GameState.IN_COUNTDOWN)
        {
            HideCountdown();
        }
        else
        {
            ShowCountdown();
        }
    }

    private void Instance_GameStateChanged(object sender, TCMiniGameStateManager.GameStateChangedArgs e)
    {
        if (e.gameState != TCMiniGameStateManager.GameState.IN_COUNTDOWN)
        {
            HideCountdown();
        }
        else
        {
            ShowCountdown();
        }
    }

    private void Update()
    {
        if (GeneralGameManager.Instance.GetCurrentChosenMinigame() == GeneralGameManager.Minigames.TOWER_CLIMB)
        {
            if (TCMiniGameStateManager.Instance.GameIsInCountdown())
            {
                text.text = Mathf.Ceil(TCMiniGameStateManager.Instance.GetCountdown()).ToString();
            }
        }
        else if (GeneralGameManager.Instance.GetCurrentChosenMinigame() == GeneralGameManager.Minigames.LETSGLIDE)
        {
            if (GlidingGameManager.Instance.GameIsInCountdown())
            {
                text.text = Mathf.Ceil(GlidingGameManager.Instance.GetCountdown()).ToString();
            }
        }
        
    }

    private void ShowCountdown()
    {
        gameObject.SetActive(true);
    }

    private void HideCountdown()
    {
        gameObject.SetActive(false);
    }
}
