using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SummonEnemy : MonoBehaviour
{
    [SerializeField] private Enemy enemyToActivate;
    [SerializeField] private Enemy enemy;
    void Start()
    {
        enemy.OnDeath += ActivateEnemy;
    }
    
    private void ActivateEnemy()
    {
        enemyToActivate.tutorial = false;
        Destroy(gameObject);
    }

    private void OnDestroy()
    {
        enemy.OnDeath -= ActivateEnemy;
    }
}
