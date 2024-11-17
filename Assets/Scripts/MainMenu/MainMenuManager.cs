using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class MainMenuManager : MonoBehaviour
{
    public LocalizationLanguage language;

    public void PlayGame()
    {
        SceneManager.LoadScene("Level1_MilitaryBase");
    }
    public void Prototype()
    {
        SceneManager.LoadScene("SampleScene");
    }

    public void BTN_Language()
    {
        LocalizationManager.Instance.ChangeLanguage(language);
    }
}
