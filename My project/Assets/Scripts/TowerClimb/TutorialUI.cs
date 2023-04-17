using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class TutorialUI : MonoBehaviour
{
    [SerializeField] private Image tutorialImage;
   
    void Start()
    {
        Show();
        if (GeneralGameManager.Instance.GetCurrentChosenMinigame() == GeneralGameManager.Minigames.TOWER_CLIMB)
        {
            TCMiniGameStateManager.Instance.GameStateChanged += Instance_GameStateChanged;
        }
        else if (GeneralGameManager.Instance.GetCurrentChosenMinigame() == GeneralGameManager.Minigames.LETSGLIDE)
        {
            GlidingGameManager.Instance.GameStateChanged += Instance_GameStateChanged1; ;
        }
    }

    private void Instance_GameStateChanged1(object sender, GlidingGameManager.GameStateChangedArgs e)
    {
        if (e.gameState == GlidingGameManager.GameState.WAITING)
        {
            Show();
        }
        else
        {
            Hide();
        }
    }

    private void Instance_GameStateChanged(object sender, TCMiniGameStateManager.GameStateChangedArgs e)
    {
        if (e.gameState == TCMiniGameStateManager.GameState.WAITING)
        {
            Show();
        }
        else
        {
            Hide();
        }
    }

    private void Hide()
    {
        tutorialImage.gameObject.SetActive(false);
    }

    private void Show()
    {
        tutorialImage.gameObject.SetActive(true);
    }
}
