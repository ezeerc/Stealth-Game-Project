using UnityEngine;
using UnityEngine.SceneManagement;

public class StaminaDecrease : MonoBehaviour
{
    //[SerializeField] private UnityEngine.UI.Button staminaDecreaseButton;
    [SerializeField] private int staminaDecrease;
    [SerializeField] private CloseButton _closeButton;

    public void OnButtonClick()
    {
        if (StaminaSystem.Instance.HasEnoughStamina(30))
        {
            StaminaSystem.Instance.UseStamina(staminaDecrease);
            if (_closeButton != null) _closeButton.Restart();
        }
        else
        {
            string panel = "WatchAdStamina";
            ScreenManager.instance.ShowScreen(panel);
        }
    }

    private void LoadScene(string sceneName)
    {
        ScreenManager.instance.HideScreen(sceneName);
    }

    public void DecreaseStamina()
    {
        StaminaSystem.Instance.UseStamina(staminaDecrease);
    }
}