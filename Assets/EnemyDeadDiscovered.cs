using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyDeadDiscovered : MonoBehaviour
{
    private void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.layer == 12)
        {
            GameManager.Instance.LoseMenu();
        }
    }
}
