using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Serialization;
using UnityEngine.UI;

public class HealthView : MonoBehaviour
{
    [SerializeField] private HealthController healthModel;
    [SerializeField] private Image healthSlider;

    private void Start()
    {
        healthModel = GameObject.FindGameObjectWithTag("Player").GetComponent<HealthController>();
        if (healthModel != null)
        {
            healthModel.HealthChanged += OnHealthChanged;
        }

        UpdateView();
    }

    private void OnDestroy()
    {
        if (healthModel != null)
        {
            healthModel.HealthChanged -= OnHealthChanged;
        }
    }

    public void UpdateView()
    {
        if (healthModel == null) return;
        
        if (healthSlider !=null && healthModel.maxHealth != 0)
        {
            healthSlider.fillAmount = (float) healthModel.actualHealth / (float)healthModel.maxHealth;
        }
    }
    
    public void OnHealthChanged()
    {
        UpdateView();
    }
}
