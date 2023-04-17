/*using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class CharacterSelectionUI : MonoBehaviour
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
}*/
