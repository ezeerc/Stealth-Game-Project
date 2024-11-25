using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using UnityEngine.UI;

public class ItemUI : MonoBehaviour
{

    [SerializeField] TextMeshProUGUI _nameTxt;
    [SerializeField] TextMeshProUGUI _costTxt;
    [SerializeField] Image _itemImage;

    public void BuildButton(ItemDTO item)
    {
        _nameTxt.text = item.itemName;
        _itemImage.sprite = item.itemIcon;
        _costTxt.text = "$"+item.itemCost.ToString();
    }

}
