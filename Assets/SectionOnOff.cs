using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SectionOnOff : MonoBehaviour
{
    [SerializeField] GameObject[] sectionsOn;
    [SerializeField] GameObject[] sectionsOff;

    private void Awake()
    {
        foreach (var element in sectionsOn)
        {
            element.SetActive(false);
        }
    }
    

    private void OnTriggerEnter(Collider other)
    {
        GameManager.Instance.SaveGame();
        if (other.gameObject.layer == 6)
        {
            foreach (var element in sectionsOn)
            {
                element.SetActive(true);
            }
            
            foreach (var element in sectionsOff)
            {
                element.SetActive(false);
            }
        }
    }
}
