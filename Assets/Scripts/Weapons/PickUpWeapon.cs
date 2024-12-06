using System;
using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class PickUpWeapon : MonoBehaviour
{
    [SerializeField, Tooltip("number 0 - Shotgun; number 1 - Uzi; number 2 - AK47; number 3 - M5; number 4 - Sniper; number 5 - pistol; number 6 - nothing")] 
    private int number;
    private void OnCollisionEnter(Collision other)
    {
        if (other.gameObject.CompareTag("Player"))
        {
                var player = other.gameObject.GetComponent<Player>();
                player.gameObject.GetComponent<WeaponController>().ChangeWeapon(number);
                Destroy(this.gameObject);
        }
    }
}
