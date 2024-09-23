using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu (fileName = "SoldierStats", menuName = "ScriptableObjects/SoldierStats")]
public class SoldierStats : ScriptableObject
{
    public float baseSpeed;
    public float runSpeed;
    public int damage;
    public float armour;
    public float fireRate;
}
