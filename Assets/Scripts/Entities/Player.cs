using System;
using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;

public class Player : Entity, IDamageable
{
    private static readonly int Sneak = Animator.StringToHash("Sneak");
    private static readonly int Run = Animator.StringToHash("Run");
    private static readonly int Strangling = Animator.StringToHash("Strangling");

    public MovementController movementController;
    public WeaponController weaponController;

    public Controller moveController;
    public Controller aimMoveController;
    public HealthController healthController;

    public Transform Target { get; set; }
    [field: SerializeField] public bool Sneaking { get; set; }
    [field: SerializeField] public bool CanStrangling { get; set; }
    [field: SerializeField] public bool CanHide { get; set; }
    [field: SerializeField] public bool InitAttack { get; set; }
    private bool _oneTimeAnimDead;
    private int _weaponAnim;

    [field: SerializeField] public int Speed { get; set; }
    
    [Header("Audio")]
    [SerializeField] private AudioClip deathSound;
    private AudioSource _audioSource;

    private Rigidbody _rb;
    private Animator _animator;
    private bool _frozen = false;
    private ICanShoot _canShoot;
    public bool Dead { get; set; }

    public LayerMask playerMask;

    public int minHealth;

    public event Action OnStrangling;
    public event Action OnHidingOn;
    public event Action OnHidingOff;

    public static Action OnStealthAttack;
    public static Action OnHideMovement;

    public static Action OnDeath;

    public void Configure(Controller controller, Controller aimController)
    {
        moveController = controller;
        aimMoveController = aimController;

        movementController = GetComponent<MovementController>();
        weaponController = GetComponent<WeaponController>();
        healthController = GetComponent<HealthController>();
        _audioSource = GetComponent<AudioSource>();

        _rb = GetComponent<Rigidbody>();
        _animator = GetComponent<Animator>();

        _canShoot = aimController.GetComponent<ICanShoot>();

        Target = this.transform.GetChild(0);

        movementController.Configure(_animator, _rb, Speed);
        healthController.Configure(minHealth, Health);

        gameObject.layer = LayerMask.NameToLayer("Player");
    }


    public void InitPlayer(PlayerBuilder builder)
    {
        builder.Build(this);
        this.Configure(moveController, aimMoveController);
        if (weaponController != null)
        {
            weaponController.OnWeaponChanged += SetWeaponAnimation;
        }
        GameManager.Instance.OnRestart += Revive;
    }

    private void FixedUpdate()
    {
        Move();
    }

    private void Update()
    {
        MoveAim();
        Shot(_canShoot);
        Death();
    }

    public override void Move()
    {
        if (_frozen) return;
        var direction = moveController.GetMovementInput();
        movementController.Move(direction);
    }

    private void MoveAim()
    {
        var direction = aimMoveController.GetMovementInput();
        movementController.MoveAim(direction);
        CheckAimMovement(aimMoveController);
    }

    public void FrozenMove(float time)
    {
        StartCoroutine(FrozenCoroutine(time));
    }

    private IEnumerator FrozenCoroutine(float time)
    {
        _frozen = true;
        yield return new WaitForSeconds(time);
        _animator.SetBool("Strangling2", false);
        _animator.SetInteger("WeaponType_int", _weaponAnim);
        Sneaking = false;
        CanStrangling = false;
        _animator.SetBool(Sneak, false);
        _animator.SetBool(Run, false);
        _frozen = false;
    }

    private void SetWeaponAnimation(int number)
    {
        if (number != 6)
        {
            _weaponAnim = 1;
        }
        else
        {
            _weaponAnim = 0;
        }

        _animator.SetInteger("WeaponType_int", _weaponAnim);
    }

    private void Shot(ICanShoot shot)
    {
        if (shot.FireOn)
        {
            FrozenMove(0.5f);
            weaponController.Shot();
            shot.FireOn = false;
        }
    }

    public void GetFullHealth()
    {
        healthController.RestoreHealth();
    }

    private void CheckAimMovement(Controller controller)
    {
        if (controller.MovingStick)
        {
            weaponController.LaserOn = true;
            ChangeSpeed(5);
        }
        else
        {
            weaponController.LaserOn = false;
            ChangeSpeed(10);
        }
    }

    public void ChangeSpeed(int speed)
    {
        movementController.Speed = speed;
    }

    public void TakeDamage(int amount)
    {
        healthController.TakeDamage(amount);
    }

    public void CanStranglingFunc()
    {
        CanStrangling = !CanStrangling;
        OnStranglingOut();
    }

    public void OnHide(bool value)
    {
        if (value)
        {
            if (OnHidingOn != null)
                OnHidingOn();
        }
        else
        {
            if (OnHidingOff != null)
                OnHidingOff();
        }

        CanHide = value;
    }

    public void OnStranglingOut()
    {
        if (OnStrangling != null)
            OnStrangling();
    }

    private void Death()
    {
        Health = healthController.actualHealth;
        if (Health <= 0)
        {
            if (!_oneTimeAnimDead)
            {
                OnDeath?.Invoke();
                PlaySound(deathSound);
                _oneTimeAnimDead = true;
                FrozenMove(2);
                _animator.SetBool(Run, false);
                _animator.SetInteger("WeaponType_int", 0);
                _animator.SetTrigger("Death");
            }
        }
    }

    public void StealthAttack()
    {
        if (CanStrangling)
        {
            //_animator.SetTrigger(Strangling);
            _animator.SetBool("Strangling2", true);
            Sneaking = false;
            CanStrangling = false;
            _animator.SetInteger("WeaponType_int", 0);
            FrozenMove(1f);
            _animator.SetBool(Run, false);
            OnStealthAttack();
        }
    }

    public void HideMovement()
    {
        if (CanHide)
        {
            _animator.SetBool("Crouch_b", true);
            StartCoroutine(HideCoroutine(1.5f));
        }
    }

    IEnumerator HideCoroutine(float time)
    {
        FrozenMove(time);
        yield return new WaitForSeconds(time);
        _animator.SetBool("Crouch_b", false);
        CanHide = false;
        OnHideMovement();
    }

    public GameMemento SaveState(List<EnemyState> enemiesState)
    {
        return new GameMemento(transform.position, Health, enemiesState);
    }

    public void RestoreState(GameMemento memento)
    {
        transform.position = memento.PlayerPosition;
        healthController.SetHealth(memento.PlayerHealth);
    }

    private void OnDestroy()
    {
        if (weaponController != null)
        {
            weaponController.OnWeaponChanged -= SetWeaponAnimation;
        }
        GameManager.Instance.OnRestart -= Revive;
    }

    private void Revive()
    {
        _oneTimeAnimDead = false;
        _animator.SetInteger("WeaponType_int", _weaponAnim);
        _animator.SetBool("DeathBool", false);
        GetFullHealth();
    }
    
    public void PlaySound(AudioClip audio)
    {
        _audioSource.clip = audio;
        _audioSource.Stop();
        _audioSource.Play();
    }

    public void PlayerCrouch(float time)
    {
        _animator.SetBool("Crouch_b", true);
        StartCoroutine(StopCrouch(time));
    }
    
    IEnumerator StopCrouch(float time)
    {
        _frozen = true;
        //FrozenMove(time);
        yield return new WaitForSeconds(time);
        _animator.SetBool("Crouch_b", false);
        _frozen = false;
    }
}