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
    [SerializeField] private Animator animator;

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
            if (healthSlider.fillAmount <= 0.4f)
            {
                animator.SetFloat("velocity", 2);
            }
            else if (healthSlider.fillAmount >= 0.41f && healthSlider.fillAmount <= 0.7f)
            {
                animator.SetFloat("velocity", 1.5f);
            }
            else if (healthSlider.fillAmount >= 0.71f)
            {
                animator.SetFloat("velocity", 1f);
            }
        }
    }
    
    public void OnHealthChanged()
    {
        UpdateView();
    }
}
