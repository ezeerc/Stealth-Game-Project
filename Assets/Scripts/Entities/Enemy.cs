using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;
using UnityEngine.TestTools;

public class Enemy : Entity, IDamageable
{
    public Transform[] wayPoints;
    public int angularSpeed = 5;
    private Animator _animator;
    private NavMeshAgent _navMeshAgent;
    private EnemyWeaponController _weaponController;
    private Rigidbody _rigidbody;

    private int currentWayPointIndex = 0;
    [SerializeField] private int _distanceToFollowPath = 3;
    public Player _player;
    public bool followPlayer = false;
    private StealthKill _stealthKill;
    private FieldOfView _fov;
    private bool _canShot = true;
    [SerializeField] private float timeBetweenAttacks = 3f;
    public bool _dead;
    [SerializeField] private RgdollController _ragdollController;


    private void Start()
    {
        Health = 40;
        _navMeshAgent = GetComponent<NavMeshAgent>();
        _navMeshAgent.speed = stats.baseSpeed;
        _animator = GetComponent<Animator>();
        _stealthKill = GetComponentInChildren<StealthKill>();
        _fov = GetComponent<FieldOfView>();
        _weaponController = GetComponent<EnemyWeaponController>();
        _rigidbody = GetComponent<Rigidbody>();
    }

    private void Update()
    {
        EnemyPath();
        if (followPlayer)
        {
            if (_player)
            {
                FollowPlayer();
            }
        }
        _animator.SetFloat("Speed_f", _navMeshAgent.velocity.magnitude);
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
        _navMeshAgent.isStopped = true;
        _stealthKill._isDead = true;
        _fov.Destroy();
    }

    public void FollowPlayer()
    {
        {
            if (_dead) return;
            if (Vector3.Distance(transform.position, _player.transform.position) <= 20)
            {
                GameManager.Instance.ChangeDetectionState(2); // Esta entrega esta hardcodeado. Planeamos usar eventos

                var destination = _navMeshAgent.SetDestination(_player.transform.position);
                if (Vector3.Distance(transform.position, _player.transform.position) <= 10)
                {
                    //GetDistanceFromPlayer(_navMeshAgent, _player.transform);
                    //FaceTarget(_player);
                    //_navMeshAgent.isStopped = true;
                    _animator.SetBool("Jump_b", true);
                    print("pego patada");
                    if (!_canShot) return;
                    CoroutineManager.Instance.StartCoroutine(FreezeCoroutine(timeBetweenAttacks));
                }
                else if (Vector3.Distance(transform.position, _player.transform.position) <= 10)
                {
                    _animator.SetBool("Jump_b", true);
                    print("pego patada");

                    //_navMeshAgent.isStopped = false;
                }
            }
            else if (Vector3.Distance(transform.position, _player.transform.position) > 20)
            {
                followPlayer = false;
                GameManager.Instance.ChangeDetectionState(0); // hardcodeado
            }
        }
    }

    private void EnemyPath()
    {
        if (wayPoints.Length == 1)
        {
            if (Vector3.Distance(transform.position, wayPoints[currentWayPointIndex].position) > 2)
            {
                SteeringBehaviors.Seek(_navMeshAgent, wayPoints[currentWayPointIndex].position);
            }
            else
            {
                //_navMeshAgent.isStopped = true;
            }
        }
        
        else if (wayPoints.Length > 1)
        {
            SteeringBehaviors.Seek(_navMeshAgent, wayPoints[currentWayPointIndex].position);

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
    }

    private void GetDistanceFromPlayer(NavMeshAgent navMeshAgent, Transform target)
    {
        if (Vector3.Distance(this.transform.position, target.transform.position) <= 10)
        {
            SteeringBehaviors.Flee(navMeshAgent, new Vector3(target.transform.position.x, target.transform.position.y, target.transform.position.z));
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
            FaceTarget(_player);
            _canShot = false;
            _weaponController.Shot();
            //_navMeshAgent.isStopped = false;
            yield return new WaitForSeconds(time);
            _canShot = true;
        }
    }

    void FaceTarget(Player player)
    {
        Vector3 direction = player.transform.position - transform.position;  
        float angle = Mathf.Atan2(direction.x, direction.z) *  Mathf.Rad2Deg;
        _rigidbody.rotation = Quaternion.Euler(transform.rotation.eulerAngles.y, angle, angularSpeed * Time.deltaTime);
    }

    public void GetPlayer(Player player)
    {
        _player = player;
        followPlayer = true;
    }
    
}