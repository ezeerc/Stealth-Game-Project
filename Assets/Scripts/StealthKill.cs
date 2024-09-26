using System;
using UnityEngine;
using UnityEngine.AI;

public class StealthKill : MonoBehaviour
{
    private static readonly int Strangled = Animator.StringToHash("Strangled");
    private Player _player;
    private Vector3 _target;
    private Animator _animator;
    private SneakSkill _skill;
    private GameObject _enemy;
    private Vector3 _velocity = Vector3.zero;
    private SneakSkill _sneakSkill;
    private bool _isDead;
    
    

    private void Start()
    {
        _player = GameObject.FindGameObjectWithTag("Player").GetComponent<Player>();
        _animator = GetComponentInParent<Animator>();
        _enemy = this.transform.parent.gameObject;
    }

    private void Update()
    {
        VictimDeath();
    }

   private void OnTriggerEnter(Collider other)
    {
        if (!other.CompareTag("Player")) return;
        if (_isDead) return;
        _player.CanStranglingFunc();
        _target = other.GetComponent<Player>().Target.transform.position;
    }

    private void OnTriggerExit(Collider other)
    {
        if (!other.CompareTag("Player")) return;
        if (_isDead) return;
        _player.CanStranglingFunc();
    }

    private void VictimDeath()
    {
        if (_player.InitAttack && !_isDead && Vector3.Distance(_player.transform.position, _enemy.transform.position) < 5f)
        {
            var enemy = _enemy.GetComponent<NavMeshAgent>();
            enemy.isStopped = true;
            _enemy.transform.rotation = _player.transform.rotation;
            _isDead = true;
            _enemy.transform.position =
                Vector3.SmoothDamp(_enemy.transform.position, _target, ref _velocity, Time.deltaTime);
            _animator.SetTrigger(Strangled);
            _player.OnStranglingOut();
            _player.InitAttack = false;
        }
    }

}