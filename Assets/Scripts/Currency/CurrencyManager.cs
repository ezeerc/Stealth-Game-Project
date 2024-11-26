using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class CurrencyManager : MonoBehaviour
{
    public static CurrencyManager Instance { get; private set; }

    [SerializeField] TextMeshProUGUI _currencyTxt;
    public int currency;

    private const string CurrencyKey = "Currency";

    void Awake()
    {
        if (Instance != null && Instance != this)
        {
            Destroy(gameObject);
            return;
        }

        Instance = this;
        DontDestroyOnLoad(gameObject);

        currency = PlayerPrefs.GetInt(CurrencyKey, 50);
        UpdateCanvasCurrency();
    }

    public int AddMoney(int amount)
    {
        currency += amount;
        SaveCurrency();
        UpdateCanvasCurrency();
        return currency;
    }

    public void Reset()
    {
        currency = 50;
        SaveCurrency();
        UpdateCanvasCurrency();
    }

    public int SubtractMoney(int amount)
    {
        currency -= amount;
        SaveCurrency();
        UpdateCanvasCurrency();
        return currency;
    }

    public void UpdateCanvasCurrency()
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

    private void SaveCurrency()
    {
        PlayerPrefs.SetInt(CurrencyKey, currency);
        PlayerPrefs.Save();
    }
}