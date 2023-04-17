using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class RingsPassedUI : MonoBehaviour
{
    [SerializeField] private TextMeshProUGUI playerRingsPassed;
    [SerializeField] private PlayerCharacter player;

    private void Start()
    {
        //TCMiniGameStateManager.Instance.GameStateChanged += Instance_GameStateChanged;
        playerRingsPassed.text = "0 / " + RingSpawner.Instance.CalculateAmountOfRings().ToString();
    }

    

    private void Instance_GameStateChanged(object sender, TCMiniGameStateManager.GameStateChangedArgs e)
    {
        if (e.gameState == TCMiniGameStateManager.GameState.PLAYING)
        {
            ShowPlayerRings();
        }
        else
        {
            HidePlayerRings();
        }
    }

    private void ShowPlayerRings()
    {
        gameObject.SetActive(true);
    }

    private void HidePlayerRings()
    {
        gameObject.SetActive(false);
    }
}
