using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CallEnemy : MonoBehaviour
{
    [SerializeField] private Enemy _enemy;
    private Player _player;
    [SerializeField] private float detectionRadius;
    
    private void Start()
    {
        Collider[] colliders = Physics.OverlapSphere(transform.position, detectionRadius, LayerMask.GetMask("Player"));
    
        if (colliders.Length > 0)
        {
            _player = colliders[0].GetComponent<Player>();
        }

        if (_player != null)
        {
            _enemy.OnDeath += OnEnemyDeath;
        }
    }

    private void OnEnemyDeath()
    {
        _player.TakeDamage(100);
        _enemy.OnRestart();
        _enemy.Revive();
    }

    private void OnDestroy()
    {
        _enemy.OnDeath -= OnEnemyDeath;
    }
}
