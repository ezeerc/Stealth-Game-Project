using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WinMenu : MonoBehaviour, IScreen
{
    [SerializeField] private GameObject WinPanel;

    public void Show()
    {
        //WinPanel.SetActive(true);
        //Time.timeScale = 0f; // Pausar el juego
        GameManager.Instance.StartCoroutine(WaitTimeForGameOver(1.5f));
    }

    IEnumerator WaitTimeForGameOver(float time)
    {
        yield return new WaitForSeconds(time);
        WinPanel.SetActive(true);
        Time.timeScale = 0f; // Pausar el juego
    }

    public void Hide()
    {
        WinPanel.SetActive(false);
        Time.timeScale = 1f; // Reanudar el juego
    }

    private void Start()
    {
        ScreenManager.instance.RegisterScreen("WinScreen", this);
        ScreenManager.instance.HideScreen("WinScreen");
    }

}
