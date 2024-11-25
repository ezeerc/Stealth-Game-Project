using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using UnityEngine.UI;
using System;

public class ItemUI : MonoBehaviour
{

    [SerializeField] TextMeshProUGUI _nameTxt;
    [SerializeField] TextMeshProUGUI _costTxt;
    [SerializeField] Image _itemImage;
    public UnityEngine.UI.Button buyButton;
    public UnityEngine.UI.Button equipButton;
    public UnityEngine.UI.Button unequipButton;

    public event Action<ItemDTO, ItemUI> onItemClickedBuy;
    public event Action<ItemDTO, ItemUI> onItemClickedEquip;
    public event Action<ItemDTO, ItemUI> onItemClickedUnequip;
    ItemDTO _itemToRepresent;

    public void BuildButton(ItemDTO item)
    {
        _nameTxt.text = item.itemName;
        _itemImage.sprite = item.itemIcon;
        _costTxt.text = "$"+item.itemCost.ToString();
        _itemToRepresent = item;
    }

    public ItemDTO GetItemDTO()
    {
        return _itemToRepresent;
    }

    public void OnClickBuy()
    {
        onItemClickedBuy?.Invoke(_itemToRepresent, this);
    }


    public void OnClickEquip()
    {
        onItemClickedEquip?.Invoke(_itemToRepresent, this);
    }

    public void OnClickUnequip()
    {
        onItemClickedUnequip?.Invoke(_itemToRepresent, this);
    }
}
