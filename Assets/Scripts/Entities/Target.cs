using UnityEngine;
using UnityEngine.AI;
using System;
using System.Collections;

public class Target : Enemy, IDamageable
{
    [Header("Settings")]
    [SerializeField] private float enemyDistanceRun = 15f;
    [SerializeField] private Vector3 targetEnd;

    [Header("Status Flags")]
    private bool _firstLook = false;
    private bool _isDead = false;

    [Header("Components")]
    private Animator _animator;
    private StealthKill _stealthKill;
    private RgdollController _ragdollController;
    
    public static event Action OnTargetDeath;
    public static event Action TargetWon;

    private void Start()
    {
        InitializeComponents();
        _player = GameObject.FindGameObjectWithTag("Player").GetComponent<Player>();
    }

    private void InitializeComponents()
    {
        _ragdollController = GetComponent<RgdollController>();
        _animator = GetComponent<Animator>();
        NavMeshAgent = GetComponent<NavMeshAgent>();
        Rigidbody = GetComponent<Rigidbody>();
        _stealthKill = GetComponentInChildren<StealthKill>();
    }

    private void Update()
    {
        UpdateAnimatorSpeed();
        float distanceToPlayer = Vector3.Distance(transform.position, _player.transform.position);

        if (ShouldFleeFromPlayer(distanceToPlayer))
        {
            _firstLook = true;
            FleeFromPlayer();
        }
        else if (ShouldMoveToTarget(distanceToPlayer))
        {
            NavMeshAgent.SetDestination(targetEnd);
        }

        CheckIfTargetReached();
    }

    private void UpdateAnimatorSpeed()
    {
        _animator.SetFloat("Speed_f", NavMeshAgent.velocity.magnitude);
    }

    private bool ShouldFleeFromPlayer(float distance)
    {
        return distance < enemyDistanceRun && GameManager.Instance.detectionState == GameManager.DetectionState.Detected;
    }

    private bool ShouldMoveToTarget(float distance)
    {
        return distance > enemyDistanceRun && distance < 50 && _firstLook;
    }

    public void TakeDamage(int amount)
    {
        if (Health <= 0) return;

        Health -= amount;

        if (Health <= 0)
        {
            _isDead = true;
            OnTargetDeath?.Invoke();
            Ragdoll(1f);
        }
    }

    public void Ragdoll(float delay)
    {
        CoroutineManager.Instance.StartCoroutine(RagdollCoroutine(delay));
    }

    private IEnumerator RagdollCoroutine(float delay)
    {
        yield return new WaitForSeconds(delay);
        _ragdollController.ActivateRagdoll();
        NavMeshAgent.isStopped = true;
        //_stealthKill._isDead = true;
        DisableComponentsForRagdoll();
    }

    private void DisableComponentsForRagdoll()
    {
        GetComponent<CapsuleCollider>().enabled = false;
        GetComponent<Animator>().enabled = false;
    }

    private void CheckIfTargetReached()
    {
        if (Vector3.Distance(transform.position, targetEnd) < 1.5f)
        {
            TargetWon?.Invoke();
        }
    }
}
