using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class SceneLoaderInputs : MonoBehaviour
{
    public GameObject loadingScreen;
    public Slider progressBar;
    public TextMeshProUGUI progressText;
    public TextMeshProUGUI tipsText;
    public SceneLoader loader;
    private void Start()
    {
        GetSceneLoader();
        SceneLoader.Instance.GetLoadingScreen(loadingScreen);
        SceneLoader.Instance.GetSlider(progressBar);
        SceneLoader.Instance.GetProgressText(progressText);
        SceneLoader.Instance.GetTipsText(tipsText);
    }

    private void GetSceneLoader()
    {
        loader = SceneLoader.Instance;
    }

    public void SetScene(string sceneName)
    {
        loader.SetSceneName(sceneName);
    }

    public void LoadSceneWithName()
    {
        loader.LoadSceneWithName();
    }

    public void LoadScene(string sceneName)
    {
        loader.LoadScene(sceneName);
    }
}
