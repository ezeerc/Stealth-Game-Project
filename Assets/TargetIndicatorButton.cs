using System;
using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.SceneManagement;

public class TargetIndicatorButton : MonoBehaviour
{
    private bool activated = false;
    public DirectionArrow directionArrow;
    private void Start()
    {
        DisableObject();
        DirectionArrow.OnCreatedArrow += GetArrow;
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
        if (!activated)
        {
            activated = true;
            directionArrow.ActivateArrow();
        }
        else
        {
            activated = false;
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
