using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class Target : Enemy, IDamageable
{
    private NavMeshAgent _agent;
    private GameObject _player;
    public float enemyDistanceRun = 8f;
    [SerializeField] private Vector3 targetEnd;
    private bool _firstLook;
    private Animator _animator;
    public bool _isDead;
    public static Action OnTargetDeath;
    private StealthKill _stealthKill;
    public static Action TargetWon;
    private void Start()
    {
        _agent = GetComponent<NavMeshAgent>();
        _player = GameObject.FindWithTag("Player");
        _animator = GetComponent<Animator>();
    }

    private void Update()
    {
        _animator.SetFloat("Speed_f", _agent.velocity.magnitude);
        float distance = Vector3.Distance(transform.position, _player.transform.position);

        if (distance < enemyDistanceRun && GameManager.Instance.detectionState == GameManager.DetectionState.Detected)
        {
            Vector3 dirToPlayer = transform.position - _player.transform.position;
            Vector3 newPos = transform.position + dirToPlayer;
            _agent.SetDestination(newPos);
            _firstLook = true;
        }
        else if (distance > enemyDistanceRun && distance < 50 && _firstLook)
        {
            _agent.speed = 10;
            _agent.SetDestination(targetEnd);
        }

        Evade();
    }

    public void TakeDamage(int amount)
    {
        if (Health > 0)
        {
            Health -= amount;
        }
        else if (Health <= 0)
        {
            OnTargetDeath?.Invoke();
        }
    }
    
    IEnumerator RagdollCoroutine(float time)
    {
        yield return new WaitForSeconds(time);
        GetComponent<CapsuleCollider>().enabled = false;
        GetComponent<Animator>().enabled = false;
        _agent.isStopped = true;
        _stealthKill._isDead = true;
    }

    public void Ragdoll(float time)
    {
        CoroutineManager.Instance.StartCoroutine(RagdollCoroutine(time));
    }

    private void Evade()
    {
        if (Vector3.Distance(transform.position, targetEnd) < 1.5f)
        {
            TargetWon();
        }
    }
}