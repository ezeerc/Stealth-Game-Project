using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SummonEnemy : MonoBehaviour
{
    [SerializeField] private GameObject enemyToActivate;
    [SerializeField] private Enemy enemy;
    void Start()
    {
        enemy.OnDeath += ActivateEnemy;
    }
    
    private void ActivateEnemy()
    {
        print("funca");
        enemy.SeenDead = true;
        enemyToActivate.SetActive(true);
    }

    private void OnDestroy()
    {
        enemy.OnDeath -= ActivateEnemy;
    }
}
