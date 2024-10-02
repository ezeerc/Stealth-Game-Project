using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.EventSystems;

public class CloseButton : MonoBehaviour
{
    [SerializeField] GameObject panel;
    
    public void ClosePanel()
    {
        panel.SetActive(false);
    }
}
