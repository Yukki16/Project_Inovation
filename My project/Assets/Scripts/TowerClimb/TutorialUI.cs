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
        TCMiniGameStateManager.Instance.GameStateChanged += Instance_GameStateChanged; 
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
