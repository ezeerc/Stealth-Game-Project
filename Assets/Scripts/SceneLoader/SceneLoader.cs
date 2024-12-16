using System;
using System.Collections;
using TMPro;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;
using Random = UnityEngine.Random;

public class SceneLoader : MonoBehaviour
{
    [Header("UI Elements")]
    private GameObject _loadingScreen;
    private Slider _progressBar;
    private TextMeshProUGUI _progressText;
    private TextMeshProUGUI _tipsText;
    public string[] spanishTips;
    public string[] englishTips;
    public string sceneToLoad;

    public float delayBeforeActivation = 1.0f;
    
    public static SceneLoader Instance { get; private set; }

    private void Awake()
    {
        if (Instance != null && Instance != this)
        {
            Destroy(gameObject);
            return;
        }

        Instance = this;
        DontDestroyOnLoad(gameObject);
    }

    public void GetLoadingScreen(GameObject screen)
    {
        _loadingScreen = screen;
    }
    
    public void GetSlider(Slider slider)
    {
        _progressBar = slider;
    }

    public void GetTipsText(TextMeshProUGUI text)
    {
        _tipsText = text;
    }

    public void GetProgressText(TextMeshProUGUI text)
    {
        _progressText = text;
    }
    public void LoadScene(string sceneName)
    {
        StartCoroutine(LoadSceneAsync(sceneName));
    }

    public void SetSceneName(string sceneName)
    {
        sceneToLoad = sceneName;
    }
    
    public void LoadSceneWithName()
    {
        StartCoroutine(LoadSceneAsync(sceneToLoad));
    }

    private IEnumerator LoadSceneAsync(string sceneName)
    {
        // Activar la pantalla de carga
        _loadingScreen.SetActive(true);

        ChangeTipsLanguage(GetLanguage());
        
        // Comenzar la carga asincrónica
        AsyncOperation operation = SceneManager.LoadSceneAsync(sceneName);

        // Evitar que la escena se active automáticamente
        operation.allowSceneActivation = false;

        while (!operation.isDone)
        {
            // Calcular el progreso (de 0 a 1)
            float progress = Mathf.Clamp01(operation.progress / 0.9f);

            // Actualizar la barra de progreso y el texto
            if (_progressBar != null)
                _progressBar.value = progress;

            if (_progressText != null)
                _progressText.text = (progress * 100).ToString("F0") + "%";

            // Si el progreso alcanza el 100% (0.9f en realidad indica que terminó)
            if (operation.progress >= 0.9f)
            {
                // Retraso opcional antes de activar la escena
                yield return new WaitForSecondsRealtime(delayBeforeActivation);

                // Permitir que la nueva escena se active
                operation.allowSceneActivation = true;
            }

            yield return null; // Esperar al siguiente frame
        }
    }

    private void ChangeTipsLanguage(string language)
    {
        if (language == "english")
        {
            if (_tipsText != null && englishTips.Length > 0)
            {
                // Mostrar un texto aleatorio de los consejos
                _tipsText.text = englishTips[Random.Range(0, englishTips.Length)];
            }
        }
        
        if (language == "spanish")
        {
            if (_tipsText != null && spanishTips.Length > 0)
            {
                // Mostrar un texto aleatorio de los consejos
                _tipsText.text = spanishTips[Random.Range(0, spanishTips.Length)];
            }
        }
    }

    private string GetLanguage()
    {
        var language = LocalizationManager.Instance.language;
        var languageSelected = language.ToString().ToLower();
        return languageSelected;
    }
}


