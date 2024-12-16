using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WatchAdStaminaMainMenu : MonoBehaviour, IScreen
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
        ScreenManager.instance.RegisterScreen("WatchAdStaminaMainMenu", this);
        ScreenManager.instance.HideScreen("WatchAdStaminaMainMenu");
    }

    /*IEnumerator RegisterScreenCoroutine()
    {
        yield return new WaitForSecondsRealtime(0.4f);
        ScreenManager.instance.RegisterScreen("WatchAdStaminaMainMenu", this);
        ScreenManager.instance.HideScreen("WatchAdStaminaMainMenu");
    }*/

}

