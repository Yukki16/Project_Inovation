using Assets.Scripts;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class ScoreUI : MonoBehaviour
{
    [SerializeField] private TextMeshProUGUI playerScore;
    [SerializeField] private TextMeshProUGUI increaseAmountText;
    [SerializeField] private TextMeshProUGUI decreaseAmountText;
    [SerializeField] private IPlayer player;

    private const float INCREASEAMOUNTSHOWEDTIMERMAX = 2;
    private const float DECREASEAMOUNTSHOWEDTIMERMAX = 2;
    private float increaseAmountShowedTimer;
    private float decreaseAmountShowedTimer;

    private void Start()
    {
        TCMiniGameStateManager.Instance.GameStateChanged += Instance_GameStateChanged;
        //player.PointsUpdated += Player_PointsUpdated;

        playerScore.text = "0";
        ResetTextUpdates();

        increaseAmountShowedTimer = INCREASEAMOUNTSHOWEDTIMERMAX;
        decreaseAmountShowedTimer = DECREASEAMOUNTSHOWEDTIMERMAX;

        HidePlayerScore();
    }

    //private void Player_PointsUpdated(object sender, Player.PointsUpdateArgs e)
    //{
    //    ResetTextUpdates();
    //    //if (e.type == Player.PointsUpdateArgs.UpdateTypes.POINTDECREASE) 
    //    //{
    //    //    decreaseAmountShowedTimer = DECREASEAMOUNTSHOWEDTIMERMAX - Time.deltaTime;
    //    //    decreaseAmountText.text = $"-{e.pointAmount}";
    //    //}
    //    //else if (e.type == Player.PointsUpdateArgs.UpdateTypes.POINTINCREASE)
    //    //{
    //    //    increaseAmountShowedTimer = INCREASEAMOUNTSHOWEDTIMERMAX - Time.deltaTime;
    //    //    increaseAmountText.text = $"+{e.pointAmount}";
    //    //}
    //}

    private void Instance_GameStateChanged(object sender, TCMiniGameStateManager.GameStateChangedArgs e)
    {
        if (e.gameState == TCMiniGameStateManager.GameState.PLAYING)
        {
            ShowPlayerScore();
        }
        else
        {
            HidePlayerScore();
        }
    }

    void Update()
    {
        if (TCMiniGameStateManager.Instance.GameIsPlaying())
        {
            //playerScore.text = $"{player.GetScore()}";
            CheckIncreaseAmount();
            CheckDecreaseAmount();
        }
    }

    private void ShowPlayerScore()
    {
        gameObject.SetActive(true);
    }

    private void HidePlayerScore()
    {
        gameObject.SetActive(false);
    }

    private void CheckIncreaseAmount()
    {
        if (increaseAmountShowedTimer != INCREASEAMOUNTSHOWEDTIMERMAX) //Timer is activated
        {
            increaseAmountShowedTimer -= Time.deltaTime;
            if (increaseAmountShowedTimer <= 0)
            {
                ResetTextUpdates();
            }
        }
    }

    private void CheckDecreaseAmount()
    {
        if (decreaseAmountShowedTimer != DECREASEAMOUNTSHOWEDTIMERMAX) //Timer is activated
        {
            decreaseAmountShowedTimer -= Time.deltaTime;
            if (decreaseAmountShowedTimer <= 0)
            {
                ResetTextUpdates();
            }
        }
    }

    private void ResetTextUpdates()
    {
        decreaseAmountText.text = "";
        increaseAmountText.text = "";
        increaseAmountShowedTimer = INCREASEAMOUNTSHOWEDTIMERMAX;
        decreaseAmountShowedTimer = DECREASEAMOUNTSHOWEDTIMERMAX;
    }
}
