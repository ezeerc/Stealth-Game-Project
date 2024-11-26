using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "Item", menuName = "ScriptableObjects/Shop Item", order = 0)]

public class ItemDTO : ScriptableObject
{
    public string itemID; //agregado para trabajar con playerprefs - EZE
    public string itemName;
    public Sprite itemIcon;
    public int itemCost;
    
    // Aca tiene que ir la logica que cambia el arma en el juego
}
