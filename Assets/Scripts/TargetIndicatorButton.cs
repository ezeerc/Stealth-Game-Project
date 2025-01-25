using System;
using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.SceneManagement;

public class TargetIndicatorButton : MonoBehaviour
{
    private bool _activated;
    public DirectionArrow directionArrow;

    private void Awake()
    {
        DirectionArrow.OnCreatedArrow += GetArrow;
    }

    private void Start()
    {
        DisableObject();
    }

    void DisableObject()
    {
        if (SceneManager.GetActiveScene().buildIndex == 3)
        {
            this.gameObject.SetActive(false);
        }
    }

    public void OnClickActivate()
    {
        if (!directionArrow) return;
        if (!_activated)
        {
            _activated = true;
            directionArrow.ActivateArrow();
        }
        else
        {
            _activated = false;
            directionArrow.DeactivateArrow();
        }
    }

    void GetArrow(DirectionArrow target)
    {
        directionArrow = target;
    }

    private void OnDestroy()
    {
        DirectionArrow.OnCreatedArrow -= GetArrow;
    }

}
