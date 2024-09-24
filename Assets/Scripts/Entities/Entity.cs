using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public abstract class Entity : MonoBehaviour
{
    [field: SerializeField] public int Health { get; set; }

    public SoldierStats stats;


    public virtual void Move()
    {
        
    }
    
}
