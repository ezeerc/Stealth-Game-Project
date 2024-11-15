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
    [field: SerializeField] public bool InitAttack { get; set; }

    [field: SerializeField] public int Speed { get; set; }

    private Rigidbody _rb;
    private Animator _animator;
    private bool _frozen = false;
    private ICanShoot _canShoot;
    public bool Dead { get; set; }

    public LayerMask playerMask;

    public int minHealth;

    public event Action OnStrangling;

    public static Action OnStealthAttack;

    public static Action OnDeath;

    public void Configure(Controller controller, Controller aimController)
    {
        moveController = controller;
        aimMoveController = aimController;

        movementController = GetComponent<MovementController>();
        weaponController = GetComponent<WeaponController>();
        healthController = GetComponent<HealthController>();

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

    public void FrozenMove(int time)
    {
        StartCoroutine(FrozenCoroutine(time));
    }

    private IEnumerator FrozenCoroutine(int time)
    {
        _frozen = true;
        yield return new WaitForSeconds(time);
        _animator.SetInteger("WeaponType_int", 1);
        Sneaking = false;
        CanStrangling = false;
        _animator.SetBool(Sneak, false);
        _animator.SetBool(Run, false);
        _frozen = false;
    }

    private void Shot(ICanShoot shot)
    {
        if (shot.FireOn)
        {
            FrozenMove(1);
            weaponController.Shot();
            shot.FireOn = false;
        }
    }

    private void CheckAimMovement(Controller controller)
    {
        if (controller.MovingStick)
        {
            weaponController.LaserOn = true;
            ChangeSpeed(10);
        }
        else
        {
            weaponController.LaserOn = false;
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

    public void OnStranglingOut()
    {
        if (OnStrangling != null)
            OnStrangling();
    }

    private void Death()
    {
        Health = healthController.actualHealth;
        if (Health <= 0) OnDeath?.Invoke();
    }

    public void StealthAttack()
    {
        if (CanStrangling)
        {
            _animator.SetTrigger(Strangling);
            Sneaking = false;
            CanStrangling = false;
            _animator.SetInteger("WeaponType_int", 0);
            FrozenMove(2);
            _animator.SetBool(Run, false);
            OnStealthAttack();
        }
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
}