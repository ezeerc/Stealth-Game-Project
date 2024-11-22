using UnityEngine;

public class WelcomeMessage : MonoBehaviour, IScreen
{
    [SerializeField] private GameObject welcomeMessagePanel;

    public void Show()
    {
        welcomeMessagePanel.SetActive(true);
        Time.timeScale = 0f; // Pausar el juego
    }

    public void Hide()
    {
        welcomeMessagePanel.SetActive(false);
        Time.timeScale = 1f; // Reanudar el juego
    }

    private void Start()
    {
        ScreenManager.instance.RegisterScreen("WelcomeMessage", this);
        ScreenManager.instance.HideScreen("WelcomeMessage");
    }
    
}

