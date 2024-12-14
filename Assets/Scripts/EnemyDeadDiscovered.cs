using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyDeadDiscovered : MonoBehaviour
{
    [SerializeField] private Rigidbody rb; 
    private void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.layer == 12)
        {
            GameManager.Instance.LoseMenu();
        }
        
        if (other.gameObject.layer == 6)
        {
            rb.isKinematic = true;
        }
    }
}
