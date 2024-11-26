using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WatchAdStamina : MonoBehaviour, IScreen
{
    [SerializeField] private GameObject adstaminaPanel;

    public void Show()
    {
        adstaminaPanel.SetActive(true);
    }

    public void Hide()
    {
        adstaminaPanel.SetActive(false);
    }

    private void Start()
    {
        ScreenManager.instance.RegisterScreen("WatchAdStamina", this);
        ScreenManager.instance.HideScreen("WatchAdStamina");
    }

}
