using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class MainMenuManager : MonoBehaviour
{
    public void PlayGame()
    {
        SceneManager.LoadScene("Level1_MilitaryBase");
    }
    public void Prototype()
    {
        SceneManager.LoadScene("SampleScene");
    }
}
