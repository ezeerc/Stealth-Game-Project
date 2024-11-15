using System;
using UnityEngine;
using UnityEngine.AI;

public class StealthKill : MonoBehaviour
{
    private Player _player;
    private GameObject _enemy;
    
    [SerializeField] private Enemy _enemyScript;
    private void Start()
    {
        Player.OnStealthAttack += Death;
        _player = GameObject.FindGameObjectWithTag("Player").GetComponent<Player>();
    }
    private void OnTriggerEnter(Collider other)
    {
        if (!other.CompareTag("Player")) return;
        if (_enemyScript.Dead) return;
        _player = other.GetComponent<Player>();
        _player.CanStranglingFunc();
    }

    private void OnTriggerExit(Collider other)
    {
        if (!other.CompareTag("Player")) return;
        if (_enemyScript.Dead) return;
        _player.CanStranglingFunc();
    }

    private void Death()
    {
        if (Vector3.Distance(this.transform.position, _player.transform.position) < 5)
        {
            _enemyScript.StealthDeath(_player);
        }
    }

    private void OnDestroy()
    {
        Player.OnStealthAttack -= Death;
    }
}