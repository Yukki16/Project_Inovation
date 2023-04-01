using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class GameStoppedUI : MonoBehaviour
{
    [SerializeField] private TextMeshProUGUI text;

    private void Start()
    {
        HideGameOver();
        TCMiniGameStateManager.Instance.GameStateChanged += Instance_GameStateChanged;
    }

    private void Instance_GameStateChanged(object sender, TCMiniGameStateManager.GameStateChangedArgs e)
    {
        if (e.gameState == TCMiniGameStateManager.GameState.STOPPED)
        {
            text.text = GameManager.Instance.GetNicknameOfHightestHightPlayer();
            ShowGameOver();
        }
        else
        {
            HideGameOver();
        }
    }

    private void ShowGameOver()
    {
        gameObject.SetActive(true);
    }

    private void HideGameOver()
    {
        gameObject.SetActive(false);
    }
}
