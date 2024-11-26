using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class CurrencyText : MonoBehaviour
{
    [SerializeField] private TextMeshProUGUI _currencyText;
    void Start()
    {
        CurrencyManager.Instance.SetCurrencyText(_currencyText);
    }
}
