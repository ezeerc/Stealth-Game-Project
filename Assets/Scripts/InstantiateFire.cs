using System;
using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.EventSystems;
using Update = UnityEngine.PlayerLoop.Update;

public class InstantiateFire : Joystick
{
    [SerializeField] private GameObject cancelCollider;
    private bool _cancel = false;
    public Weapon _weapon;
    
    void OnTriggerEnter2D(Collider2D other) 
    {
        _cancel = true;
    }

    private void OnTriggerExit2D(Collider2D other)
    {
        _cancel = false;
    }

    public void InstantiateBullet()
    {
        if (_cancel) return;
        _weapon.Shot();
    }
    
}
