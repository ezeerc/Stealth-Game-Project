using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WatchAdMainMenu : MonoBehaviour, IScreen
{
    [SerializeField] private GameObject adCurrencyPanel;

    public void Show()
    {
        adCurrencyPanel.SetActive(true);
    }

    public void Hide()
    {
        adCurrencyPanel.SetActive(false);
    }

    private void Start()
    {
        ScreenManager.instance.RegisterScreen("WatchAdMainMenu", this);
        ScreenManager.instance.HideScreen("WatchAdMainMenu");
    }
}