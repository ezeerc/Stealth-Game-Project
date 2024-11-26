using UnityEngine;
using UnityEngine.SceneManagement;

public class MainMenuManager : MonoBehaviour
{
    [SerializeField] private int _stamineperLevel1;

    public void PlayTutorial()
    {
        SceneManager.LoadScene("Level_Tutorial");
    }

    public void PlayLvl_1()
    {
        if (StaminaSystem.Instance._currentStamina >= _stamineperLevel1)
        {
            SceneManager.LoadScene("Level1_MilitaryBase");
        }
        else
        {
            Debug.Log("Not enough Stamina!");
        }
    }

    public void PlayLvl_2()
    {
        SceneManager.LoadScene("Level1_MilitaryBase");
    }

    public void Prototype()
    {
        SceneManager.LoadScene("SampleScene");
    }
}