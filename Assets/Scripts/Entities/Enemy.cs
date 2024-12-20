using System;
using System.Collections;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.AI;
using Random = UnityEngine.Random;


public class Enemy : Entity, IDamageable, ISoundObserver
{
    [Header("Player Interaction")] public Transform _player;
    public bool followPlayer = false;
    private float _distanceToPlayer;
    public bool SeenDead { get; set; }
    public bool canShoot = true;
    public Action OnDeath;
    

    [Header("Waypoint navigation")] public Transform[] waypoints;
    public int currentWaypointIndex = 0;
    public int distanceToFollowPath = 2;

    [Header("Enemy components")] public Animator _animator;
    public NavMeshAgent navMeshAgent;
    private EnemyWeaponController _weaponController;
    public RgdollController _ragdollController;

    [SerializeField] private GameObject enemy;
    public bool tutorial;
    private FieldOfView _fov;

    [Header("Enemy Status")] public float timeBetweenAttacks = 3f;
    public bool Dead { get; set; }
    public IEnemyBehavior currentBehavior;
    private bool _enableBody;
    private bool _canBeHide;
    private bool _stealthDeathOn = false;

    [Header("Animations")] private static readonly int Strangled = Animator.StringToHash("Strangled");
    
    [Header("Audio")]
    protected AudioSource _source;
    [SerializeField] private AudioClip _getClipStealthDeath;
    [SerializeField] private AudioClip[] _getClipDeath;
    
    public Quaternion initialRotation;
    public Vector3 initialPosition;
    private void Start()
    {
        GameManager.Instance.OnRestart += OnRestart;
        InitializeComponents();
        
        initialRotation = transform.rotation;
        initialPosition = transform.position;
        
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
        _source = GetComponent<AudioSource>();
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
                if (!_stealthDeathOn)
                {
                    var audio = _getClipDeath[Random.Range(0, _getClipDeath.Length)];
                    GetSfx(audio);
                }
                
                RagdollActivate();
                ResetDetectionState();
                Dead = true;
                StartCoroutine(ActiveCanBeHide(1f));
                OnDeath?.Invoke();
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
        //navMeshAgent.isStopped = true;
        if (_fov) _fov.enabled = false;
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
        if (!followPlayer || _player == null || Dead || tutorial) return;
        _distanceToPlayer = Vector3.Distance(transform.position, _player.position);

        if (_distanceToPlayer <= 25)
        {
            DetectFriends();
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

    public float detectionRadius = 20f;
    public LayerMask detectionLayerFriendEnemies;

    private void DetectFriends()
    {
        if (!Dead)
        {
            Collider[] colliders =
                Physics.OverlapSphere(transform.position, detectionRadius, detectionLayerFriendEnemies);

            foreach (Collider collider in colliders)
            {
                if (collider.gameObject.layer == LayerMask.NameToLayer("Enemy"))
                {
                    var enemy = collider.GetComponent<Enemy>(); 
                    if(enemy) enemy.GetPlayer(_player);
                    break;
                }
            }
        }
    }
    
    private void ResetDetectionState()
    {
        followPlayer = false;
        StartCoroutine(WaitFoResetDetectionState(10));
    }

    public void ChangeDetectionStateForTime(int time)
    {
        GameManager.Instance.ChangeDetectionState(2);
        StartCoroutine(WaitFoResetDetectionState(time));
    }

    IEnumerator WaitFoResetDetectionState(int time)
    {
        yield return new WaitForSeconds(time);
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
            _stealthDeathOn = true;
            TakeDamage(100);
            player.OnStranglingOut();
            GetSfx(_getClipStealthDeath);
        }
    }

    public void HideBody(Player player)
    {
        if (Dead && _canBeHide)
        {
            if (!_enableBody)
            {
                _enableBody = true;
                enemy.SetActive(false);
            }
        }
    }

    public void ResetEnemyCheckpoint()
    {
        Revive();
        InitializeComponents();
        _ragdollController.DeactivateRagdoll();
        navMeshAgent.isStopped = false;
        if (_fov) _fov.enabled = true;
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
    
    public void GetSfx(AudioClip customClip = null)
    {
        AudioClip clipToPlay = customClip;

        if (clipToPlay == null)
        {
            Debug.LogWarning("No se asignó audio para reproducir");
            return;
        }
        
        if (_source.clip != clipToPlay || !_source.isPlaying)
        {
            _source.clip = clipToPlay;
            _source.Play();
        }
    }
    
    public void OnSoundDetected(Vector3 position)
    {
        if (!Dead)
        {
            MoveToSound(position);
        }
    }
    
    private void MoveToSound(Vector3 position)
    {
        RotateTowards(position);
        navMeshAgent.SetDestination(position);
        SetBehavior(new InvestigateBehavior());
    }
    
    private void RotateTowards(Vector3 targetPosition)
    {
        Vector3 direction = (targetPosition - transform.position).normalized;
        Quaternion lookRotation = Quaternion.LookRotation(new Vector3(direction.x, 0, direction.z));
        transform.rotation = Quaternion.Slerp(transform.rotation, lookRotation, Time.deltaTime * navMeshAgent.angularSpeed / 2);
    }
    
    public void Revive()
    {
        if (Dead)
        {
            Dead = false;
            Health = 1;
            transform.position = initialPosition;
            transform.rotation = initialRotation;
            
            _ragdollController.DeactivateRagdoll();
            
            navMeshAgent.isStopped = false;
            if (_fov) _fov.enabled = true;
            
            ResetDetectionState();
            SetBehavior(new PatrolBehavior());
            
            enemy.SetActive(true);
            
            _enableBody = false;
            _canBeHide = false;
            
            _animator.Rebind();
            _animator.Update(0);
        }
        
        
        
        
    }

    
    public void OnRestart()
    {
        followPlayer = false;
        _player = null;
        
        transform.position = initialPosition;
        transform.rotation = initialRotation;
        
        navMeshAgent.isStopped = true;
        
        SetBehavior(new PatrolBehavior());
        
        navMeshAgent.isStopped = false;
        
        ResetDetectionState();
    }


    private void OnDestroy()
    {
        GameManager.Instance.OnRestart -= OnRestart;
    }
}