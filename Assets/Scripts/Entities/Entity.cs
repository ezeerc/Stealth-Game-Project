using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public abstract class Entity : MonoBehaviour
{
    [field: SerializeField] public int Health { get; set; }
    [field: SerializeField] public int Speed{get;set;}


    public virtual void Move()
    {
        
    }
    
}
