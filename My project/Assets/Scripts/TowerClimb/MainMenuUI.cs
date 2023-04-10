using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class MainMenuUI : MonoBehaviour
{
    [SerializeField] private Button playButton;
    [SerializeField] private Button quitButton;

    private void Awake()
    {
        playButton.onClick.AddListener(PlayClick);
        quitButton.onClick.AddListener(QuitClick);
        Time.timeScale = 1.0f;
    }
    private void PlayClick()
    {
        Loader.Load(Loader.Scene.Lobby);
    }

    private void QuitClick()
    {
        Application.Quit();
    }
}
