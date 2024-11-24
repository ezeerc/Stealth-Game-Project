using System;
using System.Collections;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.AI;


public class Enemy : Entity, IDamageable
{
    [Header("Player Interaction")]
    public Transform _player;
    public bool followPlayer = false;
    private float _distanceToPlayer;
    public bool canShoot = true;
    
    [Header("Waypoint navigation")]
    public Transform[] waypoints;
    public int currentWaypointIndex = 0;
    public int distanceToFollowPath = 2;
    
    [Header("Enemy components")]
    public Animator _animator;
    public NavMeshAgent navMeshAgent;
    private EnemyWeaponController _weaponController;
    public RgdollController _ragdollController;
    [SerializeField] private GameObject enemy;
    //protected Rigidbody Rigidbody;
    //private StealthKill _stealthKill;
    private FieldOfView _fov;
    
    [Header("Enemy Status")] 
    public float timeBetweenAttacks = 3f;
    public bool Dead { get; set; }
    public IEnemyBehavior currentBehavior;
    private bool _enableBody;
    private bool _canBeHide;

    [Header("Animations")] 
    private static readonly int Strangled = Animator.StringToHash("Strangled");
    private void Start()
    {
        InitializeComponents();
        SetBehavior(new PatrolBehavior());
    }

    private void InitializeComponents()
    {
        navMeshAgent = GetComponent<NavMeshAgent>();
        navMeshAgent.speed = stats.baseSpeed;
        _animator = GetComponent<Animator>();
        _fov = GetComponent<FieldOfView>();
        _weaponController = GetComponent<EnemyWeaponController>();
        _ragdollController = GetComponent<RgdollController>();
    }

    private void Update()
    {
        UpdateFollowPlayer();
        UpdateAnimatorSpeed();
        if (!Dead)
        {
            currentBehavior?.Execute(this);
        }
    }

    public void SetBehavior(IEnemyBehavior behavior)
    {
        currentBehavior = behavior;
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
                StartCoroutine(ActiveCanBeHide(1f));
            }
        }
    }


    IEnumerator ActiveCanBeHide(float time)
    {
        yield return new WaitForSeconds(time);
        _canBeHide = true;
    }
    public void RagdollActivate()
    {
        _ragdollController.ActivateRagdoll();
        navMeshAgent.isStopped = true;
        if(_fov) _fov.enabled = false;
    }
    
    private void UpdateAnimatorSpeed()
    {
        _animator.SetFloat("Speed_f", navMeshAgent.velocity.magnitude);
    }
    
    public IEnumerator ShootCoroutine(float delay)
    {
        if (Health >= 0)
        {
            canShoot = false;
            _weaponController.Shot();
            yield return new WaitForSeconds(delay);
            canShoot = true;
        }
    }
    
    public void GetPlayer(Transform player)
    {
        _player = player;
        followPlayer = true;
    }

    public void UpdateFollowPlayer()
    {
        if (!followPlayer || _player == null || Dead) return;
        DetectFriends();

        _distanceToPlayer = Vector3.Distance(transform.position, _player.position);

        if (_distanceToPlayer <= 25)
        {
            GameManager.Instance.ChangeDetectionState(2);

            if (_distanceToPlayer <= 20 && _distanceToPlayer >= 6)
            {
                SetBehavior(new EngageBehavior());
            }
            else if (_distanceToPlayer <= 5)
            {
                SetBehavior(new FleeBehavior());
            }
        }
        else if (_distanceToPlayer > 24)
        {
            SetBehavior(new PatrolBehavior());
            ResetDetectionState();
        }
    }
    
    public float detectionRadius = 1f;
    public LayerMask detectionLayerFriendEnemies;

    public void DetectFriends()
    {
        // Checkea si hay objetos en el radio de detección
        Collider[] colliders = Physics.OverlapSphere(transform.position, detectionRadius, detectionLayerFriendEnemies);
        
        foreach (Collider collider in colliders)
        {
            if (collider.gameObject.layer == LayerMask.NameToLayer("Enemy"))
            {
                Enemy enemy = collider.GetComponent<Enemy>();
                enemy.GetPlayer(_player);
                break;
            }
        }
    }

    private void OnDrawGizmosSelected()
    {
        Gizmos.color = new Color(1, 0, 0, 1F);
        Gizmos.DrawSphere(transform.position, detectionRadius);
    }

    private void ResetDetectionState()
    {
        followPlayer = false;
        GameManager.Instance.ChangeDetectionState(0);
    }

    public void SetRageMode(float speed, float acceleration, int angularSpeed, int stoppingDistance,
        bool updateRotation)
    {
        navMeshAgent.speed = speed;
        navMeshAgent.acceleration = acceleration;
        navMeshAgent.angularSpeed = angularSpeed;
        navMeshAgent.stoppingDistance = stoppingDistance;
        navMeshAgent.updateRotation = updateRotation;
    }
   
    public void StealthDeath(Player player)
    {
        if (!Dead)
        {
            TakeDamage(100);
            player.OnStranglingOut();
        }
    }
    
    public void HideBody(Player player)
    {
        if (Dead && _canBeHide)
        {
            if(!_enableBody)
            {
                _enableBody = true;
                //player.OnHide();
                //player.CanHideFunc();
                print("funca ocultar");
                enemy.SetActive(false);
            }
        }
    }

    public void ResetEnemyCheckpoint()
    {
        _player = null;
        _ragdollController.DeactivateRagdoll();
        navMeshAgent.isStopped = false;
        if(_fov) _fov.enabled = true;
        ResetDetectionState();
        SetBehavior(new PatrolBehavior());
    }
    public EnemyState SaveState()
    {
        return new EnemyState(transform.position, Health, Dead);
    }

    public void RestoreState(EnemyState state)
    {
        transform.position = state.Position;
        Health = state.Health;
        Dead = state.IsDead;
        if (!Dead)
        {
            ResetEnemyCheckpoint();
        }
    }

}