using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ScoreBoard : MonoBehaviour
{
    [SerializeField] GameObject scoreBoardBlack;
    [SerializeField] GameObject scoreBoardWhite;
    [SerializeField] GameObject scoreBoardOrange;
    [SerializeField] GameObject scoreBoardPurple;

    [SerializeField] Toggle[] blackScore;
    [SerializeField] Toggle[] whiteScore;
    [SerializeField] Toggle[] orangeScore;
    [SerializeField] Toggle[] purpleScore;

    private void Start()
    {
        StartCoroutine(DysplayScoreBoard());
    }

    IEnumerator DysplayScoreBoard()
    {
        yield return new WaitForSeconds(1f);

        foreach(var client in GeneralGameManager.Instance.GetClientsWithTheirCharacterColor())
        {
            if(client.Value == null)
            {
                switch (client.Key)
                {
                    case GeneralGameManager.CharacterColors.BLACK:
                        scoreBoardBlack.SetActive(false);
                        break;
                    case GeneralGameManager.CharacterColors.WHITE:
                        scoreBoardWhite.SetActive(false);
                        break;
                    case GeneralGameManager.CharacterColors.ORANGE:
                        scoreBoardOrange.SetActive(false);
                        break;
                    case GeneralGameManager.CharacterColors.PURPLE:
                        scoreBoardPurple.SetActive(false);
                        break;
                }
                continue;
            }
            foreach(var connectedClients in GeneralGameManager.Instance.ReturnClientsPoints())
            {
                if (connectedClients.Key == client.Value)
                {
                    switch(client.Key)
                    {
                        case GeneralGameManager.CharacterColors.BLACK:
                            for(int i = 0; i < connectedClients.Value; i++)
                            {
                                blackScore[i].isOn = true;
                            }
                            break;
                        case GeneralGameManager.CharacterColors.WHITE:
                            for (int i = 0; i < connectedClients.Value; i++)
                            {
                                whiteScore[i].isOn = true;
                            }
                            break;
                        case GeneralGameManager.CharacterColors.ORANGE:
                            for (int i = 0; i < connectedClients.Value; i++)
                            {
                                orangeScore[i].isOn = true;
                            }
                            break;
                        case GeneralGameManager.CharacterColors.PURPLE:
                            for (int i = 0; i < connectedClients.Value; i++)
                            {
                                purpleScore[i].isOn = true;
                            }
                            break;
                    }
                }
            }
        }
        yield return null;
    }
}
