using UnityEngine;

public class PauseMenu : MonoBehaviour, IScreen
{
    [SerializeField] private GameObject pausePanel;

    public void Show()
    {
        pausePanel.SetActive(true);
        Time.timeScale = 0f; // Pausar el juego
    }

    public void Hide()
    {
        pausePanel.SetActive(false);
        Time.timeScale = 1f; // Reanudar el juego
    }

    private void Start()
    {
        ScreenManager.instance.RegisterScreen("PauseMenu", this);
    }
}

