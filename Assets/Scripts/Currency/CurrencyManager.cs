using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class CurrencyManager : MonoBehaviour
{
    public static CurrencyManager Instance { get; private set; }

    [SerializeField] TextMeshProUGUI _currencyTxt;
    public int currency;

    void Awake()
    {
        if (Instance != null && Instance != this)
        {
            Destroy(gameObject);
            return;
        }

        Instance = this;
        DontDestroyOnLoad(gameObject);

        UpdateCanvasCurrency();
    }

    public int AddMoney(int amount)
    {
        currency += amount;
        UpdateCanvasCurrency();
        return currency;
    }

    public int SubtractMoney(int amount)
    {
        currency -= amount;
        UpdateCanvasCurrency();
        return currency;
    }

    private void UpdateCanvasCurrency()
    {
        if (_currencyTxt != null)
        {
            _currencyTxt.text = currency.ToString();
        }
    }
    
    public void SetCurrencyText(TextMeshProUGUI currencyTxt)
    {
        _currencyTxt = currencyTxt;
        UpdateCanvasCurrency();
    }
}
