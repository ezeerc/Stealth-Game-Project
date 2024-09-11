using System;
using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.EventSystems;

public class InstantiateFire : MonoBehaviour
{
    private void OnPointerExit(PointerEventData eventData)
    {
        InstantiateLaser();
    }
    public void InstantiateLaser()
    {
            Debug.Log("Piuuu, piuuuuu");
    }
    
    public void CancelLaser()
    {
        Debug.Log("Laser cancelado");
    }

    private void OnEndDrag(PointerEventData eventData)
    {
        Ray ray = Camera.main.ScreenPointToRay( Input.GetTouch(0).position );
        RaycastHit hit;

        if (!Physics.Raycast(ray, out hit) || hit.transform.gameObject.name != "AimJoystick")
            Debug.Log("funca");
    }
    /*private bool Shot()
    {
        if (Input.touchCount > 0 && Input.GetTouch (0).phase == TouchPhase.Ended) 
        {
            Ray ray = Camera.main.ScreenPointToRay( Input.GetTouch(0).position );
            RaycastHit hit;

            if (!Physics.Raycast(ray, out hit) || hit.transform.gameObject.name != "AimJoystick") return true;
            Debug.Log("Funca");
            return false;
        }

        return true;
    }*/

    private void Update()
    {
    }
}
