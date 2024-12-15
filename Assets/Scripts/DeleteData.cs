using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DeleteData : MonoBehaviour
{
    [SerializeField] ShopBuilder shopBuilder;
    [SerializeField] ShopController shopController;
    public void Delete()
    {
        PlayerPrefs.DeleteAll();
        shopBuilder.ResetShop();
        shopController.ConnectToShop();
        CurrencyManager.Instance.Reset();
        StaminaSystem.Instance.ResetStamina();
    }
}
