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
            newItem.onItemClicked += OnItemBought;
        }
    }

    void OnItemBought(ItemDTO itemToBuy)
    {
        /*
        if (CurrencyManager.Instance.Currency >= itemToBuy.itemCost)
        {
            CurrencyManager.Instance.Currency -= itemToBuy.itemCost;
        }
        else
            Debug.Log("Not enough money.");
        */
    }
}
