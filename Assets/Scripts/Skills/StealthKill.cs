using System;
using UnityEngine;
using UnityEngine.AI;

public class StealthKill : MonoBehaviour
{
    private static readonly int Strangled = Animator.StringToHash("Strangled");
    private Player _player;
    private Vector3 _target;
    private bool _oneTime = false;

    private Animator _animator;
    
    private GameObject _enemy;

    private Vector3 _velocity = Vector3.zero;

    [SerializeField] private Enemy _enemyScript;


    private void Start()
    {
        Player.OnStealthAttack += Death;
        _player = GameObject.FindGameObjectWithTag("Player").GetComponent<Player>();
        _animator = GetComponentInParent<Animator>();
    }
    private void OnTriggerEnter(Collider other)
    {
        if (!other.CompareTag("Player")) return;
        if (_enemyScript.Dead) return;
        _player = other.GetComponent<Player>();
        _player.CanStranglingFunc();
        _target = other.GetComponent<Player>().Target.transform.position;
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
}