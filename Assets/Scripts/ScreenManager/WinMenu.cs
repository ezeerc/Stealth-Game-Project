using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WinMenu : MonoBehaviour, IScreen
{
    [SerializeField] private GameObject WinPanel;
    [SerializeField] private GameObject doublePrizePanel;
    private bool _oneTime = false;

    public void Show()
    {
        //WinPanel.SetActive(true);
        //Time.timeScale = 0f; // Pausar el juego
        GameManager.Instance.StartCoroutine(WaitTimeForGameOver(1.5f));
        GameManager.Instance.StartCoroutine(DoublePrize(5f));
    }

    IEnumerator WaitTimeForGameOver(float time)
    {
        yield return new WaitForSeconds(time);
        WinPanel.SetActive(true);
        Time.timeScale = 0f; // Pausar el juego
    }

    IEnumerator DoublePrize(float time)
    {
        if (!_oneTime)
        {
            _oneTime = true;
            yield return CoroutineUtil.WaitForRealSeconds(time);
            doublePrizePanel.SetActive(true);
        }
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