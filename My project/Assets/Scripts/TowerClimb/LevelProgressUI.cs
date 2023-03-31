using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class LevelProgressUI : MonoBehaviour
{
    [SerializeField] private Image slider;
    [SerializeField] private Transform player;
    [SerializeField] private Transform cat;
    [SerializeField] private TextMeshProUGUI progressInMeters;

    void Start()
    {
        TCMiniGameManager.Instance.GameStateChanged += Instance_GameStateChanged;
        HideLevelProgress();
        slider.fillAmount= 0;
        progressInMeters.text = "0m";
    }

    private void Instance_GameStateChanged(object sender, TCMiniGameManager.GameStateChangedArgs e)
    {
        if (e.gameState == TCMiniGameManager.GameState.PLAYING)
        {
            ShowLevelProgress();
        }
        else
        {
            HideLevelProgress();
        }
    }

    // Update is called once per frame
    void Update()
    {
        if (TCMiniGameManager.Instance.GameIsPlaying())
        {
            float amountToAddToBar = player.position.y / GameManager.Instance.GetMaxGameLength();
            slider.fillAmount = amountToAddToBar;
            progressInMeters.text = $"{(int)player.position.y}m";
        }
    }

    private void ShowLevelProgress()
    {
        gameObject.SetActive(true);
    }

    private void HideLevelProgress()
    {
        gameObject.SetActive(false);
    }
}
