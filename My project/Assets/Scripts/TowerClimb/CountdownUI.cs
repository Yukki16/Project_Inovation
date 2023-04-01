using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class CountdownUI : MonoBehaviour
{
    [SerializeField] private TextMeshProUGUI text;

    private void Awake()
    {
        ShowCountdown();
    }

    private void Start()
    {
        TCMiniGameStateManager.Instance.CountdownStopped += Instance_CountdownStopped;
    }

    private void Update()
    {
        if (TCMiniGameStateManager.Instance.GameIsInCountdown())
        {
            text.text = Mathf.Ceil(TCMiniGameStateManager.Instance.GetCountdown()).ToString();
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
