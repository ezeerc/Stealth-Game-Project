using System;
using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class DeactivateWall : MonoBehaviour
{
    private void OnTriggerEnter(Collider other)
    {
        if (other.GameObject().layer == LayerMask.NameToLayer("Enemy"))
        {
            Destroy(gameObject);
        }
    }
}
