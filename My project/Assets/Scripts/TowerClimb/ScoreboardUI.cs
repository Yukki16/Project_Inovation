using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class ScoreboardUI : MonoBehaviour
{
    [SerializeField] private TextMeshProUGUI scoresText;

    private void Start()
    {
        HideScoreboard();
        //TCMiniGameStateManager.Instance.GameStateChanged += Instance_GameStateChanged;
    }

    //private void Instance_GameStateChanged(object sender, TCMiniGameStateManager.GameStateChangedArgs e)
    //{
    //    if (e.gameState == TCMiniGameStateManager.GameState.SHOWING_WINNER)
    //    {
    //        string scoreStringText = "";
    //        Dictionary<string,int> scoreboardResults = GameManager.Instance.GetScoreboard();
    //        foreach (string key in scoreboardResults.Keys)
    //        {
    //            scoreStringText += $"{key}\t\t{scoreboardResults[key]}\n";
    //        }
    //        scoresText.text = scoreStringText;
    //        ShowScoreboard();
    //    }
    //    else
    //    {
    //        HideScoreboard();
    //    }
    //}

    private void ShowScoreboard()
    {
        gameObject.SetActive(true);
    }

    private void HideScoreboard()
    {
        gameObject.SetActive(false);
    }
}
