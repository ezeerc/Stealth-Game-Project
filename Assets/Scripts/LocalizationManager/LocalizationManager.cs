using System;
using System.Collections.Generic;
using UnityEngine;

public class LocalizationManager : MonoBehaviour
{
    public static LocalizationManager Instance { get; private set; }

    public DataLocalization[] data;

    Dictionary<LocalizationLanguage, Dictionary<string, string>> _translate = new Dictionary<LocalizationLanguage, Dictionary<string, string>>();

    //public SystemLanguage unityLanguage;    <-- Lo trae unity y tiene muchos idiomas, pero no es tan practico tener tantos si nuestro juego solo va a tener dos idiomas
    public LocalizationLanguage language;
    public event Action EventChangeLanguage;


    private void Awake()
    {
        if (Instance != null && Instance != this)
        {
            Destroy(gameObject);
            return;
        }

        Instance = this;
        DontDestroyOnLoad(gameObject);
        _translate = LanguageU.LoadTranslation(data);
    }


    public void ChangeLanguage(LocalizationLanguage newLang)
    {
        if (language == newLang)
            return;

        language = newLang;

        if (EventChangeLanguage != null)
            EventChangeLanguage();
    }
    
    public string GetTranslation(string ID)
    {
        if (!_translate.ContainsKey(language))
            return "No Language";
        if (!_translate[language].ContainsKey(ID))
            return "No ID";

        return _translate[language][ID];
    }
}


public enum LocalizationLanguage
{
    English,
    Spanish
}
