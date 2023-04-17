using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ProgressBar : MonoBehaviour
{
    public static ProgressBar Instance { get; private set; }

    [SerializeField] Slider blackCat;
    [SerializeField] Slider whiteCat;
    [SerializeField] Slider orangeCat;
    [SerializeField] Slider noHairCat;

    private Dictionary<GeneralGameManager.CharacterColors, Transform> players = new Dictionary<GeneralGameManager.CharacterColors, Transform>();
    public void GetAndUpdatePlayers()
    {
        if (GeneralGameManager.Instance.GetCurrentChosenMinigame() == GeneralGameManager.Minigames.TOWER_CLIMB)
        {
            Dictionary<GeneralGameManager.CharacterColors, Transform> newPlayerList = new Dictionary<GeneralGameManager.CharacterColors, Transform>();
            foreach (var client in NetworkManager.Instance.GetAllPlayers())
            {
                newPlayerList.Add((client as TCPLayer).GetCharacterColor(), (client as TCPLayer).GetPlayerBody());
            }
            players = newPlayerList;
        }
        else if (GeneralGameManager.Instance.GetCurrentChosenMinigame() == GeneralGameManager.Minigames.LETSGLIDE)
        {
            Dictionary<GeneralGameManager.CharacterColors, Transform> newPlayerList = new Dictionary<GeneralGameManager.CharacterColors, Transform>();
            foreach (var client in NetworkManager.Instance.GetAllPlayers())
            {
                newPlayerList.Add((client as PlayerCharacter).GetCharacterColor(), (client as PlayerCharacter).transform);
            }
            players = newPlayerList;
        }
        
    }

    private void Start()
    {
        Instance = this;
        foreach(var clients in GeneralGameManager.Instance.GetClientsWithTheirCharacterColor())
        {
            if(clients.Value != null)
            {
                switch(clients.Key)
                {
                    case GeneralGameManager.CharacterColors.BLACK:
                        blackCat.gameObject.SetActive(true);
                        break;
                    case GeneralGameManager.CharacterColors.WHITE:
                        whiteCat.gameObject.SetActive(true);
                        break;
                    case GeneralGameManager.CharacterColors.ORANGE:
                        orangeCat.gameObject.SetActive(true);
                        break;
                    case GeneralGameManager.CharacterColors.PURPLE:
                        noHairCat.gameObject.SetActive(true);
                        break;
                }
            }
        }
        GetAndUpdatePlayers();
    }

    public void UpdateProgressBarOnDisconect()
    {
        foreach (var clients in GeneralGameManager.Instance.GetClientsWithTheirCharacterColor())
        {
            if (clients.Value == null)
            {
                switch (clients.Key)
                {
                    case GeneralGameManager.CharacterColors.BLACK:
                        blackCat.gameObject.SetActive(false);
                        break;
                    case GeneralGameManager.CharacterColors.WHITE:
                        whiteCat.gameObject.SetActive(false);
                        break;
                    case GeneralGameManager.CharacterColors.ORANGE:
                        orangeCat.gameObject.SetActive(false);
                        break;
                    case GeneralGameManager.CharacterColors.PURPLE:
                        noHairCat.gameObject.SetActive(false);
                        break;
                }
            }
        }
        GetAndUpdatePlayers();
    }
    private void Update()
    {
        UpdateProgressBar();
    }
    void UpdateProgressBar()
    {
        if(GeneralGameManager.Instance.GetCurrentChosenMinigame() == GeneralGameManager.Minigames.TOWER_CLIMB)
        {
            if(TCMiniGameStateManager.Instance.ReturnCurrentGameState() == TCMiniGameStateManager.GameState.PLAYING)
            {
                foreach(var player in players)
                {
                    if (player.Value == null || player.Key == GeneralGameManager.CharacterColors.NONE)
                        continue;

                    switch(player.Key)
                    {
                        case GeneralGameManager.CharacterColors.BLACK:
                            blackCat.value = new Vector3(1f, GameManager.Instance.GetMaxGameLength() - player.Value.position.y, 1f).normalized.y;
                            break;
                        case GeneralGameManager.CharacterColors.WHITE:
                            whiteCat.value = new Vector3(1f, GameManager.Instance.GetMaxGameLength() - player.Value.position.y, 1f).normalized.y;
                            break;
                        case GeneralGameManager.CharacterColors.ORANGE:
                            orangeCat.value = new Vector3(1f, GameManager.Instance.GetMaxGameLength() - player.Value.position.y, 1f).normalized.y;
                            break;
                        case GeneralGameManager.CharacterColors.PURPLE:
                            noHairCat.value = new Vector3(1f, GameManager.Instance.GetMaxGameLength() - player.Value.position.y, 1f).normalized.y;
                            break;
                    }
                }
            }
        }
        else if (GeneralGameManager.Instance.GetCurrentChosenMinigame() == GeneralGameManager.Minigames.LETSGLIDE)
        {
            if (GlidingGameManager.Instance.ReturnCurrentGameState() == GlidingGameManager.GameState.PLAYING)
            {
                foreach (var player in players)
                {
                    if (player.Value == null || player.Key == GeneralGameManager.CharacterColors.NONE)
                        continue;

                    switch (player.Key)
                    {
                        case GeneralGameManager.CharacterColors.BLACK:
                            blackCat.value = new Vector3(1f, GlidingGameManager.Instance.GetGameLengthInMeters() - player.Value.position.y, 1f).normalized.y;
                            break;
                        case GeneralGameManager.CharacterColors.WHITE:
                            whiteCat.value = new Vector3(1f, GlidingGameManager.Instance.GetGameLengthInMeters() - player.Value.position.y, 1f).normalized.y;
                            break;
                        case GeneralGameManager.CharacterColors.ORANGE:
                            orangeCat.value = new Vector3(1f, GlidingGameManager.Instance.GetGameLengthInMeters() - player.Value.position.y, 1f).normalized.y;
                            break;
                        case GeneralGameManager.CharacterColors.PURPLE:
                            noHairCat.value = new Vector3(1f, GlidingGameManager.Instance.GetGameLengthInMeters() - player.Value.position.y, 1f).normalized.y;
                            break;
                    }
                }
            }
        }
    }
}
