using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class Enemy : Entity, IDamageable
{
    public Transform[] wayPoints;

    private Animator _animator;
    private NavMeshAgent navMeshAgent;

    private int currentWayPointIndex = 0;
    [SerializeField] private int _distanceToFollowPath = 2;
    public Player _player;
    public bool followPlayer = false;
    private StealthKill _stealthKill;
    private FieldOfView _fov;

    private void Start()
    {
        Health = 100;
        navMeshAgent = GetComponent<NavMeshAgent>();
        navMeshAgent.speed = stats.baseSpeed;
        _animator = GetComponent<Animator>();
        _stealthKill = GetComponentInChildren<StealthKill>();
        _fov = GetComponent<FieldOfView>();
    }

    private void Update()
    {
        EnemyPath();
        if (followPlayer)
        {
            FollowPlayer(_player.transform);
        }

        _animator.SetFloat("Speed_f", navMeshAgent.velocity.magnitude);
    }

    public void TakeDamage(int amount)
    {
        if (Health >= 0)
        {
            Health -= amount;
        }
        else if (Health <= 0)
        {
            Ragdoll(0);
        }
    }

    IEnumerator RagdollCoroutine(float time)
    {
        yield return new WaitForSeconds(time);
        GetComponent<CapsuleCollider>().enabled = false;
        GetComponent<Animator>().enabled = false;
        navMeshAgent.isStopped = true;
        _stealthKill._isDead = true;
        _fov.Destroy();
    }
    public void Ragdoll(float time)
    {
        CoroutineManager.Instance.StartCoroutine(RagdollCoroutine(time));
    }
    public void FollowPlayer(Transform target)
    {
        _player = target.GetComponent<Player>();

        if (Vector3.Distance(transform.position, _player.transform.position) <= 10)
        {
            var destination = navMeshAgent.SetDestination(_player.transform.position);
        }
        else if (Vector3.Distance(transform.position, _player.transform.position) > 10)
        {
            followPlayer = false;
        }
    }

    private void EnemyPath()
    {
        navMeshAgent.destination = wayPoints[currentWayPointIndex].position;

        if (Vector3.Distance(transform.position, wayPoints[currentWayPointIndex].position) <= _distanceToFollowPath)
        {
            if (wayPoints[currentWayPointIndex] != wayPoints[^1])
            {
                currentWayPointIndex++;
            }
            else
            {
                currentWayPointIndex = 0;
            }
        }
    }

    private void Dead()
    {
        Destroy(gameObject);
    }
}