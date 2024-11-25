using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class MainMenuManager : MonoBehaviour
{

    public void PlayTutorial()
    {
        SceneManager.LoadScene("Level_Tutorial");
    }

    public void PlayLvl_1()
    {
        SceneManager.LoadScene("Level1_MilitaryBase");
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
