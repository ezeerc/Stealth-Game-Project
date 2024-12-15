using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ShopBuilder : MonoBehaviour
{
    [SerializeField] ItemUI _itemPrefab;
    [SerializeField] public Transform shopParent;

    [SerializeField] ItemDTO[] _items = new ItemDTO[0];

    private ItemUI _currentlyEquippedItem;

    private const string BoughtKey = "Bought_";
    private const string EquippedKey = "Equipped";

    public bool _oneTime = false;

    void Start()
    {
        for (int i = 0; i < _items.Length; i++)
        {
            if (string.IsNullOrEmpty(_items[i].itemID))
            {
                _items[i].itemID = $"Item_{i}"; //genera el ID de cada arma de la tienda - EZE
            }
        }

        /*for (int i = 0; i < _items.Length; i++)
        {
            var newItem = Instantiate(_itemPrefab, shopParent);
            newItem.BuildButton(_items[i]);

            //Busca si el arma ya fue comprada o equipada anteriormente - EZE
            bool isBought = PlayerPrefs.GetInt(BoughtKey + _items[i].itemID, 0) == 1;
            if (isBought)
            {
                newItem.buyButton.gameObject.SetActive(false);
            }

            string equippedID = PlayerPrefs.GetString(EquippedKey, string.Empty);
            if (_items[i].itemID == equippedID)
            {
                _currentlyEquippedItem = newItem;
                newItem.unequipButton.gameObject.SetActive(true);
            }

            newItem.onItemClickedBuy += OnItemBought;
            newItem.onItemClickedEquip += OnItemEquipped;
            newItem.onItemClickedUnequip += OnItemUnequipped;
        }*/
        
        for (int i = 0; i < _items.Length; i++)
        {
            var newItem = Instantiate(_itemPrefab, shopParent);
            newItem.BuildButton(_items[i]);

            bool isBought = PlayerPrefs.GetInt(BoughtKey + _items[i].itemID, 0) == 1;
            if (isBought)
            {
                newItem.buyButton.gameObject.SetActive(false);
            }

            string equippedID = PlayerPrefs.GetString(EquippedKey, string.Empty);
            if (_items[i].itemID == equippedID)
            {
                _currentlyEquippedItem = newItem;
                newItem.unequipButton.gameObject.SetActive(true);
            }

            // No conectamos directamente aquí
            // newItem.onItemClickedBuy += OnItemBought; 

            newItem.onItemClickedEquip += OnItemEquipped;
            newItem.onItemClickedUnequip += OnItemUnequipped;
        }

    }

    public void OnItemBought(ItemDTO itemToBuy, ItemUI itemUIInstance)
    {
        _oneTime = false;
        if (CurrencyManager.Instance.currency >= itemToBuy.itemCost)
        {
            CurrencyManager.Instance.SubtractMoney(itemToBuy.itemCost);
            itemUIInstance.buyButton.gameObject.SetActive(false);

            // Guarda los estados de las armas compradas en Playerprefs - EZE
            PlayerPrefs.SetInt(BoughtKey + itemToBuy.itemID, 1);
            PlayerPrefs.Save();
        }
        else
        {
            Debug.Log("Not enough money.");
        }
    }

    void OnItemEquipped(ItemDTO itemToEquip, ItemUI itemUIInstance)
    {
        if (_currentlyEquippedItem != null && _currentlyEquippedItem != itemUIInstance)
        {
            _currentlyEquippedItem.unequipButton.gameObject.SetActive(false);
        }

        _currentlyEquippedItem = itemUIInstance;
        itemUIInstance.unequipButton.gameObject.SetActive(true);

        // Guarda los estados de la arma equipada en Playerprefs - EZE
        PlayerPrefs.SetString(EquippedKey, itemToEquip.itemID);
        PlayerPrefs.Save();
    }

    void OnItemUnequipped(ItemDTO itemToEquip, ItemUI itemUIInstance)
    {
        if (_currentlyEquippedItem == itemUIInstance)
        {
            _currentlyEquippedItem = null;
        }

        itemUIInstance.unequipButton.gameObject.SetActive(false);

        // limpia el estado del arma equipada - EZE
        PlayerPrefs.DeleteKey(EquippedKey);
        PlayerPrefs.Save();
    }

    public void ResetShop()
    {
        if (!_oneTime)
        {
            foreach (Transform child in shopParent)
            {
                Destroy(child.gameObject);
            }
            _oneTime = true;
            Start();
        }
    }
}