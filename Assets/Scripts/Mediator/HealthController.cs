using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HealthController : MonoBehaviour
{
    public event Action HealthChanged;

    private int _minHealth;
    public int maxHealth;
    public int actualHealth;

    public void Configure(int minHealth, int maximumHealth)
    {
        _minHealth = minHealth;
        maxHealth = maximumHealth;
        actualHealth = maxHealth;
    }
    public void GetHealth(int amount)
    {
        actualHealth += amount;
        actualHealth = Mathf.Clamp(actualHealth, _minHealth, maxHealth);
        UpdateHealthUI();
    }

    public void TakeDamage(int amount)
    {
        actualHealth -= amount;
        actualHealth = Mathf.Clamp(actualHealth, _minHealth, maxHealth);
        UpdateHealthUI();
    }

    public void RestoreHealth()
    {
        actualHealth = maxHealth;
        UpdateHealthUI();
    }

    private void UpdateHealthUI()
    {
        HealthChanged?.Invoke();
    }
}
