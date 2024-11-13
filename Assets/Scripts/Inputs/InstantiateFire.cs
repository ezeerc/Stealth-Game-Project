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
    public static Action CancelFireON; //observer de cancelación de láser
    public static Action CancelFireOFF; //observer de cancelación de láser
    public bool FireOn { get; set; }
    
    void OnTriggerEnter2D(Collider2D other)
    {
        _cancel = true;
        if (CancelFireON != null) CancelFireON();
    }

    private void OnTriggerStay2d(Collider other)
    {
        if (CancelFireON != null) CancelFireON();
    }

    private void OnTriggerExit2D(Collider2D other)
    {
        if (CancelFireOFF != null) CancelFireOFF();
        _cancel = false;
    }
    
    public void OnPointerUp(PointerEventData eventData)
    {
        if (_cancel) return;
        FireOn = true;
        MovingStick = false;
    }
}