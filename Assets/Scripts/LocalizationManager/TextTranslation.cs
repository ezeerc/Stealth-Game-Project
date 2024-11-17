using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

[RequireComponent (typeof(TextMeshProUGUI))]
public class TextTranslation : MonoBehaviour
{
    TextMeshProUGUI _text;
    [SerializeField] string _ID;

    private void Start()
    {
        _text = GetComponent<TextMeshProUGUI>();
        LocalizationManager.Instance.EventChangeLanguage += Translate;

        Translate();
    }

    void Translate()
    {
        _text.text = LocalizationManager.Instance.GetTranslation(_ID);
    }

    private void OnDestroy()
    {
        LocalizationManager.Instance.EventChangeLanguage -= Translate;
    }
}
