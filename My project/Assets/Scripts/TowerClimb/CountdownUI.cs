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
        TCMiniGameStateManager.Instance.GameStateChanged += Instance_GameStateChanged;
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
        if (TCMiniGameStateManager.Instance.GameIsInCountdown())
        {
            text.text = Mathf.Ceil(TCMiniGameStateManager.Instance.GetCountdown()).ToString();
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
