using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WatchAdWin : MonoBehaviour, IScreen
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
        ScreenManager.instance.RegisterScreen("WatchAdWin", this);
        ScreenManager.instance.HideScreen("WatchAdWin");
    }
}
