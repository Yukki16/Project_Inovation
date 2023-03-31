using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class CountdownUI : MonoBehaviour
{
    [SerializeField] private Image background;
    [SerializeField] private TextMeshProUGUI text;

    private void Awake()
    {
        ShowCountdown();
    }

    private void Start()
    {
        TCMiniGameManager.Instance.CountdownStopped += Instance_CountdownStopped;
    }

    private void Update()
    {
        if (TCMiniGameManager.Instance.GameIsInCountdown())
        {
            text.text = Mathf.Ceil(TCMiniGameManager.Instance.GetCountdown()).ToString();
        }
    }

    private void Instance_CountdownStopped(object sender, System.EventArgs e)
    {
        HideCountdown();
    }

    private void ShowCountdown()
    {
        gameObject.SetActive(true);
    }

    private void HideCountdown()
    {
        gameObject.SetActive(false);
    }
}
