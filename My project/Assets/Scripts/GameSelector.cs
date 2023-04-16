using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class GameSelector : MonoBehaviour
{
    [SerializeField] private Image towerClimbSelected;
    [SerializeField] private Image letsGlideSelected;
    private float screenVisibleTimer;

    private void Awake()
    {
        screenVisibleTimer = 8;
    }

    private void Start()
    {
        if (GeneralGameManager.Instance.GetCurrentChosenMinigame() == GeneralGameManager.Minigames.TOWER_CLIMB)
        {
            towerClimbSelected.gameObject.SetActive(true);
            letsGlideSelected.gameObject.SetActive(false);
        }
        else if (GeneralGameManager.Instance.GetCurrentChosenMinigame() == GeneralGameManager.Minigames.LETSGLIDE)
        {
            towerClimbSelected.gameObject.SetActive(false);
            letsGlideSelected.gameObject.SetActive(true);
        }
    }

    private void Update()
    {
        screenVisibleTimer -= Time.deltaTime;
        if (screenVisibleTimer <= 0)
        {
            GeneralGameManager.Instance.LoadMinigame();
        }
    }
}
