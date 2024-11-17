using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LanguageSelector : MonoBehaviour
{
    public LocalizationLanguage language;

    public void BTN_Language()
    {
        LocalizationManager.Instance.ChangeLanguage(language);
    }
}
