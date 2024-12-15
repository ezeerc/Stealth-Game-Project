using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class SureWinMenu : MonoBehaviour, IScreen
{
    [SerializeField] private GameObject SureWinMenuPanel;

    public void Show()
    {
        SureWinMenuPanel.SetActive(true);
    }

    public void Hide()
    {
        SureWinMenuPanel.SetActive(false);
    }

    private void Start()
    {
        ScreenManager.instance.RegisterScreen("SureWinMenu", this);
        ScreenManager.instance.HideScreen("SureWinMenu");
    }
}