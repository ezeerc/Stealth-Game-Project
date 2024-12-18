using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class OnDeathLose : MonoBehaviour
{
    private Enemy _enemy;
    private void Start()
    {
        _enemy = GetComponent<Enemy>();
        _enemy.OnDeath += Lose;
        GameManager.Instance.OnRestart += OnRestartRevive;
    }

    private void Lose()
    {
        GameManager.Instance.LoseMenu();
    }

    private void OnDestroy()
    {
        _enemy.OnDeath -= Lose;
    }

    private void OnRestartRevive()
    {
        _enemy.initialPosition = transform.position;
        _enemy.initialRotation = transform.rotation;
        _enemy.Revive();
    }
}
