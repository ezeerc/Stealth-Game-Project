using System;
using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class PickUpWeapon : MonoBehaviour
{
    private void OnCollisionEnter(Collision other)
    {
        if (other.gameObject.CompareTag("Player"))
        {
                var player = other.gameObject.GetComponent<Player>();
                player.gameObject.GetComponent<WeaponController>().ChangeWeapon(1);
                Destroy(this.gameObject);
        }
    }
}
