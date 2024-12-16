using System;
using UnityEngine;
using UnityEngine.SceneManagement;

public class StaminaDecrease : MonoBehaviour
{
    //[SerializeField] private UnityEngine.UI.Button staminaDecreaseButton;
    [SerializeField] private int staminaDecrease;
    private SceneLoader sceneLoader;
    private void Start()
    {
        sceneLoader = GameObject.Find("SceneLoaderManager").GetComponent<SceneLoader>();
    }

    public void CheckStaminaAndLoadScene()
    {
        if (sceneLoader.sceneToLoad == "MainMenu" || sceneLoader.sceneToLoad == "Level_Tutorial")
        {
            sceneLoader.LoadSceneWithName();
        }
        else
        {
            if (StaminaSystem.Instance.HasEnoughStamina(30))
            {
                StaminaSystem.Instance.UseStamina(staminaDecrease);
                sceneLoader.LoadSceneWithName();
            }
            else
            {
                if (SceneManager.GetActiveScene().name != "MainMenu")
                {
                    string panel = "WatchAdStamina";
                    ScreenManager.instance.ShowScreen(panel);
                }
                else
                {
                    string panel = "WatchAdStaminaMainMenu";
                    ScreenManager.instance.ShowScreen(panel);
                    
                }
            }
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