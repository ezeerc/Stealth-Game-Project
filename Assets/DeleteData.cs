using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DeleteData : MonoBehaviour
{
    [SerializeField] ShopBuilder _shopBuilder;
    public void Delete()
    {
        PlayerPrefs.DeleteAll();
        _shopBuilder.ResetShop();
        CurrencyManager.Instance.Reset();
        StaminaSystem.Instance.ResetStamina();
    }
}
