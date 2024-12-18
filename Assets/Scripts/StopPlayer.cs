using System;
using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class StopPlayer : MonoBehaviour
{
    private bool _oneTime = false;
    private void OnTriggerEnter(Collider other)
    {
        if (other.GameObject().layer == LayerMask.NameToLayer("Player"))
        {
            if(_oneTime == false) other.GetComponent<Player>().PlayerCrouch(30);
            _oneTime = true;
        }
    }
}
