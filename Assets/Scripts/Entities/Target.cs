using UnityEngine;
using UnityEngine.AI;
using System;
using System.Collections;

public class Target : Enemy
{
    [Header("Settings")] 
    [SerializeField] private float enemyDistanceRun = 11f;
    [SerializeField] private float fleeDistanceRun = 10f;
    [SerializeField] private Vector3 targetEnd;

    [Header("Status")] 
    private bool _firstLook = false;
    

    public static event Action OnTargetDeath;
    public static event Action TargetWon;

    private void Start()
    {
        InitializeComponents();
        _player = GameObject.FindGameObjectWithTag("Player").GetComponent<Transform>();
        SetBehavior(new IdleTargetBehavior());
        _source = GetComponent<AudioSource>();
    }

    private void InitializeComponents()
    {
        _ragdollController = GetComponent<RgdollController>();
        _animator = GetComponent<Animator>();
        navMeshAgent = GetComponent<NavMeshAgent>();
    }

    private float DistanceToPlayer(Transform player)
    {
        float distanceToPlayer = Vector3.Distance(transform.position, _player.position);
        return distanceToPlayer;
    }

    private void Update()
    {
        if (!Dead)
        {
            currentBehavior?.Execute(this);
            UpdateAnimatorSpeed();
            ChangeBehaviorBasedOnDistance(DistanceToPlayer(_player));
            CheckIfTargetReached();
        }
    }

    private void LateUpdate()
    {
        CheckForDeath();
    }

    private void UpdateAnimatorSpeed()
    {
        _animator.SetFloat("Speed_f", navMeshAgent.velocity.magnitude);
    }

    private void ChangeBehaviorBasedOnDistance(float distanceToPlayer)
    {
        if (ShouldFleeFromPlayer(distanceToPlayer))
        {
            SetBehavior(new FleeTargetBehavior());
            _firstLook = true;
        }
        else if (ShouldMoveToTarget(distanceToPlayer))
        {
            SetBehavior(new MoveToTargetBehavior(targetEnd));
        }
        else
        {
            if (_firstLook)
            {
                SetBehavior(new MoveToTargetBehavior(targetEnd));
            }
            else
            {
                SetBehavior(new IdleTargetBehavior());
            }
        }
    }

    private bool ShouldFleeFromPlayer(float distance)
    {
        return distance < fleeDistanceRun;
    }

    private bool ShouldMoveToTarget(float distance)
    {
        return distance > enemyDistanceRun && distance < 50 && _firstLook;
    }

    private void CheckForDeath()
    {
        if (Dead) OnTargetDeath?.Invoke();
    }

    private void CheckIfTargetReached()
    {
        if (Vector3.Distance(transform.position, targetEnd) < 1.5f)
        {
            TargetWon?.Invoke();
        }
    }
}