using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Net.Sockets;
using UnityEngine;

public class GameEndingUI : MonoBehaviour
{
    [SerializeField] private GameObject blackCat;
    [SerializeField] private GameObject orangeCat;
    [SerializeField] private GameObject whiteCat;
    [SerializeField] private GameObject purpleCat;

    private void Start()
    {
        HideAll();
        ShowWinner();
    }

    private void ShowWinner()
    {
        TcpClient winner = GeneralGameManager.Instance.GetWinner();
        GeneralGameManager.CharacterColors colorToShow = GeneralGameManager.Instance.GetClientsWithTheirCharacterColor().Where(color => color.Value == winner).FirstOrDefault().Key;
        switch (colorToShow)
        {
            case GeneralGameManager.CharacterColors.BLACK:
                blackCat.SetActive(true);
                break;
            case GeneralGameManager.CharacterColors.ORANGE:
                orangeCat.SetActive(true);
                break;
            case GeneralGameManager.CharacterColors.WHITE:
                whiteCat.SetActive(true);
                break;
            case GeneralGameManager.CharacterColors.PURPLE:
                purpleCat.SetActive(true);
                break;
        }
    }

    private void HideAll()
    {
        blackCat.SetActive(false);
        orangeCat.SetActive(false);
        whiteCat.SetActive(false);
        purpleCat.SetActive(false);
    }
}
