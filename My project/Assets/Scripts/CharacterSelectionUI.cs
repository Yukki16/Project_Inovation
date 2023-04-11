using System.Collections;
using System.Collections.Generic;
using Unity.Netcode;
using UnityEngine;
using UnityEngine.UI;

public class CharacterSelectionUI : NetworkBehaviour
{
    [SerializeField] private Button readyButton;

    private void Start()
    {
        Hide();
        if (NetworkManager.Singleton.IsServer)
        {
            Show();
            readyButton.onClick.AddListener(() =>
            {
                CharactersSelectReady.Instance.SetPlayerReady();
            });
        }
    }

    private void Hide()
    {
        gameObject.SetActive(false);
    }

    private void Show()
    {
        gameObject.SetActive(true);
    }
}
