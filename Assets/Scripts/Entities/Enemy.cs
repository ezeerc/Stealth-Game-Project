using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;
using UnityEngine.TestTools;

public class Enemy : Entity, IDamageable
{
    public Transform[] wayPoints;

    private Animator _animator;
    private NavMeshAgent navMeshAgent;
    private EnemyWeaponController _weaponController;

    private int currentWayPointIndex = 0;
    [SerializeField] private int _distanceToFollowPath = 2;
    public Player _player;
    public bool followPlayer = false;
    private StealthKill _stealthKill;
    private FieldOfView _fov;
    private bool _canShot = true;
    [SerializeField] private float timeBetweenAttacks = 3f;
    public bool _dead;
    [SerializeField] private RagdollController _ragdollController;


    private void Start()
    {
        Health = 40;
        navMeshAgent = GetComponent<NavMeshAgent>();
        navMeshAgent.speed = stats.baseSpeed;
        _animator = GetComponent<Animator>();
        _stealthKill = GetComponentInChildren<StealthKill>();
        _fov = GetComponent<FieldOfView>();
        _weaponController = GetComponent<EnemyWeaponController>();
    }

    private void Update()
    {
        EnemyPath();
        if (followPlayer)
        {
            if (_player != null)
            {
                FollowPlayer(_player.transform);
            }
        }

        _animator.SetFloat("Speed_f", navMeshAgent.velocity.magnitude);
        
        
    }

    public void TakeDamage(int amount)
    {
        if (Health > 0)
        {
            Health -= amount;
            if (Health <= 0)
            {
                RagdollActivate();
                _dead = true;
            }
        }
    }
    
    public void RagdollActivate()
    {
        _ragdollController.ActivateRagdoll();
        navMeshAgent.isStopped = true;
        _stealthKill._isDead = true;
        _fov.Destroy();
    }

    public void FollowPlayer(Transform target)
    {
        _player = target.GetComponent<Player>();

        if (Vector3.Distance(transform.position, _player.transform.position) <= 20 && !_dead)
        {
            GameManager.Instance.ChangeDetectionState(2); // Esta entrega esta hardcodeado. Planeamos usar eventos

            var destination = navMeshAgent.SetDestination(_player.transform.position);
            if (Vector3.Distance(transform.position, _player.transform.position) <= 10 && !_dead)
            {
                navMeshAgent.isStopped = true;
                if (!_canShot) return;
                CoroutineManager.Instance.StartCoroutine(FreezeCoroutine(timeBetweenAttacks));
            }
            else if (Vector3.Distance(transform.position, _player.transform.position) >= 10)
            {
                navMeshAgent.isStopped = false;
            }
        }
        else if (Vector3.Distance(transform.position, _player.transform.position) > 20)
        {
            followPlayer = false;
            GameManager.Instance.ChangeDetectionState(0); // hardcodeado
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

    IEnumerator FreezeCoroutine(float time)
    {
        if (Health >= 0)
        {
            FaceTarget();
            _canShot = false;
            _weaponController.Shot();
            navMeshAgent.isStopped = false;
            yield return new WaitForSeconds(time);
            _canShot = true;
        }
    }

    void FaceTarget()
    {
        var turnTowardNavSteeringTarget = navMeshAgent.steeringTarget;
        Vector3 direction = (turnTowardNavSteeringTarget - transform.position).normalized;
        Quaternion lookRotation = Quaternion.LookRotation(new Vector3(direction.x, 0, direction.z));
        transform.rotation = Quaternion.Slerp(transform.rotation, lookRotation, Time.deltaTime * 5);
    }
    
}