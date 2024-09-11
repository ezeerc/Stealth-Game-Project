using System;
using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.EventSystems;
using Update = UnityEngine.PlayerLoop.Update;

public class InstantiateFire : MonoBehaviour
{
    [SerializeField] private GameObject cancelCollider;
    private bool _cancel = false;
    
    void OnTriggerEnter2D(Collider2D other) 
    {
        _cancel = true;
        Debug.Log ("Triggered");
    }

    private void OnTriggerExit2D(Collider2D other)
    {
        _cancel = false;
    }

    public void InstantiateLaser()
    {
        if (_cancel) return;
            Debug.Log("Piuuu, piuuuuu");
    }

}
