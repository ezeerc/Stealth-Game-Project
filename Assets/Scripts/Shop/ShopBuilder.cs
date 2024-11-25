using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ShopBuilder : MonoBehaviour
{   
    [SerializeField] ItemUI _itemPrefab;
    [SerializeField] Transform shopParent;

    [SerializeField] ItemDTO[] _items = new ItemDTO[0];

    private ItemUI _currentlyEquippedItem;
    
    
    void Start()
    {
        for (int i = 0; i < _items.Length; i++)
        {
            var newItem = Instantiate(_itemPrefab, shopParent);
            newItem.BuildButton(_items[i]);

            newItem.onItemClickedBuy += OnItemBought;
            newItem.onItemClickedEquip += OnItemEquipped;
            newItem.onItemClickedUnequip += OnItemUnequipped;
        }
    }

    void OnItemBought(ItemDTO itemToBuy, ItemUI itemUIInstance)
    {
        
        if (CurrencyManager.Instance.currency >= itemToBuy.itemCost)
        {
            CurrencyManager.Instance.SubtractMoney(itemToBuy.itemCost);
            itemUIInstance.buyButton.gameObject.SetActive(false);
        }
        else
            Debug.Log("Not enough money.");
    }

    void OnItemEquipped(ItemDTO itemToEquip, ItemUI itemUIInstance)
    {
        if (_currentlyEquippedItem != null && _currentlyEquippedItem != itemUIInstance)
        {
            _currentlyEquippedItem.unequipButton.gameObject.SetActive(false);
        }

        _currentlyEquippedItem = itemUIInstance;

        itemUIInstance.unequipButton.gameObject.SetActive(true);
    }

    void OnItemUnequipped(ItemDTO itemToEquip, ItemUI itemUIInstance)
    {
        if (_currentlyEquippedItem == itemUIInstance)
        {
            _currentlyEquippedItem = null;
        }

        itemUIInstance.unequipButton.gameObject.SetActive(false);
    }
}
