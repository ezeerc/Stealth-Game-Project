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
            var player = other.gameObject.GetComponent<Player>();
            if (!_oneTime)
            {
                player.PlayerCrouch(15);
                player.FrozenMove(15);
                _oneTime = true;
            }
        }
    }
    
}
