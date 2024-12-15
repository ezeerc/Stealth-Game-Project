using System.Collections;
using TMPro;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class SceneLoader : MonoBehaviour
{
    [Header("UI Elements")]
    public GameObject loadingScreen; // Panel de la pantalla de carga
    public Slider progressBar;       // Barra de progreso
    public TextMeshProUGUI progressText;
    public TextMeshProUGUI tipsText;
    public string[] spanishTips;
    public string[] englishTips;
    public string sceneToLoad;
    
    public float delayBeforeActivation = 1.0f; // Retardo opcional antes de activar la nueva escena

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
        loadingScreen.SetActive(true);

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
            if (progressBar != null)
                progressBar.value = progress;

            if (progressText != null)
                progressText.text = (progress * 100).ToString("F0") + "%";

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
            if (tipsText != null && englishTips.Length > 0)
            {
                // Mostrar un texto aleatorio de los consejos
                tipsText.text = englishTips[Random.Range(0, englishTips.Length)];
            }
        }
        
        if (language == "spanish")
        {
            if (tipsText != null && spanishTips.Length > 0)
            {
                // Mostrar un texto aleatorio de los consejos
                tipsText.text = spanishTips[Random.Range(0, spanishTips.Length)];
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


