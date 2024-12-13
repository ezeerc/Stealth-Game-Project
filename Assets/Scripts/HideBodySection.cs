using System;
using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class HideBodySection : MonoBehaviour
{
    [SerializeField] private GameObject enemy;
    [SerializeField] private GameObject barrier;
    private void OnTriggerEnter(Collider other)
    {
        if (other.gameObject == enemy)
        {
            barrier.SetActive(false);
        }
    }
}
