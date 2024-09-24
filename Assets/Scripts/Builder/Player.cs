using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;

public class Player : Entity, IDamageable
{
    public MovementController movementController;
    public WeaponController weaponController;
    
    public Controller moveController;
    public Controller aimMoveController;
    public HealthController healthController;
    public Transform Target { get; set; }
    public bool Sneaking { get; set; }
    public bool CanStrangling { get; set; }
    public bool InitAttack { get; set; }

    [field: SerializeField] public int Speed { get; set; }

    private Rigidbody _rb;
    private Animator _animator;
    private SneakSkill _sneakSkill;
    private bool _frozen = false;
    private ICanShoot _canShoot;

    
    public int minHealth;


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
        
        _sneakSkill = new SneakSkill();
        _sneakSkill.Configure(this, _animator);
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
        _sneakSkill.GetSkill();
        MoveAim();
        Shot(_canShoot);

        if(Input.anyKeyDown)
        {
            TakeDamage(10);
        }
    }

    public override void Move()
    {
        if(_frozen) return;
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
        _frozen = false;
    }

    private void Shot(ICanShoot shot)
    {
        if (shot.FireOn)
        {
            weaponController.Shot();
            shot.FireOn = false;
        }
    }

    private void CheckAimMovement(Controller controller)
    {
        if (controller.MovingStick)
        {
            weaponController.LaserOn = true;
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

}
