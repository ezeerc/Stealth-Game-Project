using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameOverScreen : MonoBehaviour, IScreen
{
    [SerializeField] private GameObject gameOverPanel;

    public void Show()
    {
        //gameOverPanel.SetActive(true);
        //Time.timeScale = 0f; // Pausar el juego
        GameManager.Instance.StartCoroutine(WaitTimeForGameOver(1.5f));
    }

    IEnumerator WaitTimeForGameOver(float time)
    {
        yield return new WaitForSeconds(time);
        gameOverPanel.SetActive(true);
        Time.timeScale = 0f; // Pausar el juego
    }

    public void Hide()
    {
        gameOverPanel.SetActive(false);
        Time.timeScale = 1f; // Reanudar el juego
    }

    private void Start()
    {
        ScreenManager.instance.RegisterScreen("GameOverScreen", this);
        ScreenManager.instance.HideScreen("GameOverScreen");
    }

}
