using System;
using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.EventSystems;
using Update = UnityEngine.PlayerLoop.Update;

public class InstantiateFire : Joystick, IPointerUpHandler, ICanShoot
{
    [SerializeField] private GameObject cancelCollider;
    private bool _cancel = false;
    public bool FireOn { get; set; }
    
    void OnTriggerEnter2D(Collider2D other) 
    {
        _cancel = true;
    }

    private void OnTriggerExit2D(Collider2D other)
    {
        _cancel = false;
    }
    
    public void OnPointerUp(PointerEventData eventData)
    {
        if (_cancel) return;
        FireOn = true;
        MovingStick = false;
    }
}