using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SureMenu : MonoBehaviour, IScreen
{
    [SerializeField] private GameObject SureMenuPanel;

    public void Show()
    {
        SureMenuPanel.SetActive(true);
    }

    public void Hide()
    {
        SureMenuPanel.SetActive(false);
    }

    private void Start()
    {
        ScreenManager.instance.RegisterScreen("SureMenu", this);
        ScreenManager.instance.HideScreen("SureMenu");
    }
}
