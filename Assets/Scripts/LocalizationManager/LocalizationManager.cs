using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LocalizationManager : MonoBehaviour
{
    public static LocalizationManager Instance;
    
    public DataLocalization[] data;

    Dictionary<LocalizationLanguage, Dictionary<string, string>> _translate = new Dictionary<LocalizationLanguage, Dictionary<string, string>>();

    //public SystemLanguage unityLanguage;    <-- Lo trae unity y tiene muchos idiomas, pero no es tan practico tener tantos si nuestro juego solo va a tener dos idiomas
    public LocalizationLanguage language;


    private void Awake()
    {
        Instance = this;
        _translate = LanguageU.LoadTranslation(data);
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
