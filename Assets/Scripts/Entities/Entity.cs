using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public abstract class Entity : MonoBehaviour, IDamageable
{
    protected int Health;
    [field: SerializeField] public float Speed{get;set;}
    
    public void AddDamage(int amount)
    {
        Health -= amount;
    }
}
