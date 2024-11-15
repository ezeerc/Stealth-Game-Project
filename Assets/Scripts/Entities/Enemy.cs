using System;
using System.Collections;
using System.Collections.Generic;
using System.Diagnostics;
using UnityEditorInternal;
using UnityEngine;
using UnityEngine.AI;
using UnityEngine.TestTools;

public class Enemy : Entity, IDamageable
{
    [Header("Player Interaction")]
    public Player _player;
    public bool followPlayer = false;
    private float _distanceToPlayer;
    private bool _canShoot = true;
    
    [Header("Waypoint navigation")]
    public Transform[] waypoints;
    private int _currentWaypointIndex = 0;
    [SerializeField] private int _distanceToFollowPath = 3;
    
    [Header("Enemy components")]
    private Animator _animator;
    protected NavMeshAgent NavMeshAgent;
    private EnemyWeaponController _weaponController;
    private RgdollController _ragdollController;
    protected Rigidbody Rigidbody;
    private StealthKill _stealthKill;
    private FieldOfView _fov;
    
    [Header("Enemy Status")] 
    [SerializeField] private float timeBetweenAttacks = 3f;
    public bool Dead { get; set; }
    public float _initialRotation;

    [Header("Animations")] 
    private static readonly int Strangled = Animator.StringToHash("Strangled");
    private void Start()
    {
        InitializeComponents();
    }

    private void InitializeComponents()
    {
        NavMeshAgent = GetComponent<NavMeshAgent>();
        NavMeshAgent.speed = stats.baseSpeed;
        _animator = GetComponent<Animator>();
        _stealthKill = GetComponentInChildren<StealthKill>();
        _fov = GetComponent<FieldOfView>();
        _weaponController = GetComponent<EnemyWeaponController>();
        Rigidbody = GetComponent<Rigidbody>();
        _ragdollController = GetComponent<RgdollController>();
        _initialRotation = transform.rotation.eulerAngles.y;
    }

    private void Update()
    {
        UpdateEnemyPath();
        UpdateFollowPlayer();
        UpdateAnimatorSpeed();
    }

    public void TakeDamage(int amount)
    {
        if (Health > 0)
        {
            Health -= amount;
            if (Health <= 0)
            {
                RagdollActivate();
                ResetDetectionState();
                Dead = true;
            }
        }
    }
    public void RagdollActivate()
    {
        _ragdollController.ActivateRagdoll();
        NavMeshAgent.isStopped = true;
        if(_fov) _fov.Destroy();
    }

    private void UpdateFollowPlayer()
    {
        if (!followPlayer || _player == null || Dead) return;
        
        _distanceToPlayer = Vector3.Distance(transform.position, _player.transform.position);

        if (_distanceToPlayer <= 25)
        {
            GameManager.Instance.ChangeDetectionState(2);

            if (_distanceToPlayer <= 20 && _distanceToPlayer >= 6)
            {
                EngagePlayer();
            }
            else if (_distanceToPlayer <= 5)
            {
                FleeFromPlayer();
            }
        }
        else if (_distanceToPlayer > 24)
        {
            ResetDetectionState();
        }
    }
    
    private void EngagePlayer()
    {
        SetRageMode(6f, 6f, 360, 15, false);
        SteeringBehaviors.Seek(NavMeshAgent, _player.transform.position);
        transform.LookAt(_player.transform.position);

        if (_canShoot)
        {
            CoroutineManager.Instance.StartCoroutine(ShootCoroutine(timeBetweenAttacks));
        }
    }
    
    protected void FleeFromPlayer()
    {
        SetRageMode(10f, 10f, 480, 0, true);
        SteeringBehaviors.Flee(NavMeshAgent, _player.transform.position);
    }
    
    private void ResetDetectionState()
    {
        SetRageMode(2f, 5f, 120, 0, true);
        followPlayer = false;
        GameManager.Instance.ChangeDetectionState(0);
    }

    private void SetRageMode(float speed, float acceleration, int angularSpeed, int stoppingDistance,
        bool updateRotation)
    {
        NavMeshAgent.speed = speed;
        NavMeshAgent.acceleration = acceleration;
        NavMeshAgent.angularSpeed = angularSpeed;
        NavMeshAgent.stoppingDistance = stoppingDistance;
        NavMeshAgent.updateRotation = updateRotation;
    }
   
    private void UpdateEnemyPath()
    {
        if (waypoints.Length == 0) return;

        Transform targetWaypoint = waypoints[_currentWaypointIndex];
        SteeringBehaviors.Seek(NavMeshAgent, targetWaypoint.position);

        if (Vector3.Distance(transform.position, targetWaypoint.position) <= _distanceToFollowPath)
        {
            _currentWaypointIndex = (_currentWaypointIndex + 1) % waypoints.Length;
        }
    }

    private IEnumerator ShootCoroutine(float delay)
    {
        if (Health >= 0)
        {
            _canShoot = false;
            _weaponController.Shot();
            yield return new WaitForSeconds(delay);
            _canShoot = true;
        }
    }
    
    private void UpdateAnimatorSpeed()
    {
        _animator.SetFloat("Speed_f", NavMeshAgent.velocity.magnitude);
    }

    public void GetPlayer(Player player)
    {
        _player = player;
        followPlayer = true;
    }
    
    public void StealthDeath(Player player)
    {
        if (!Dead)
        {
            TakeDamage(100);
            player.OnStranglingOut();
        }
    }

}