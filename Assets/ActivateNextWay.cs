using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ActivateNextWay : MonoBehaviour
{
    [SerializeField] private Enemy enemy;
    [SerializeField] private GameObject wall;
    private Player player;

    private void Start()
    {
        player = GameObject.FindGameObjectWithTag("Player").GetComponent<Player>();
        enemy.OnDeath += OnDeathKill;
    }

    private void OnTriggerEnter(Collider other)
    {
            wall.SetActive(false);
    }

    private void OnDeathKill()
    {
        player.TakeDamage(100);
    }

    private void OnDestroy()
    {
        enemy.OnDeath -= OnDeathKill;
    }
}
