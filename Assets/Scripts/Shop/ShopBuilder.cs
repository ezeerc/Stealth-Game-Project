using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ShopBuilder : MonoBehaviour
{   
    [SerializeField] ItemUI _itemPrefab;
    [SerializeField] Transform shopParent;

    [SerializeField] ItemDTO[] _items = new ItemDTO[0];
    
    
    void Start()
    {
        for (int i = 0; i < _items.Length; i++)
        {
            var newItem = Instantiate(_itemPrefab, shopParent);
            newItem.BuildButton(_items[i]);
            newItem.onItemClicked += (item) => OnItemBought(item, newItem);
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

    }
}
