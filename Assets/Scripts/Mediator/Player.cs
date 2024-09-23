using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class Player : Entity, IDamageable
{
    [SerializeField] MovementController movementController;
    [SerializeField] WeaponController weaponController;
    
    [SerializeField] private Controller moveController;
    [SerializeField] private Controller aimMoveController;
    [SerializeField] private HealthController healthController;

    private ICanShoot _canShoot;
    public bool Sneaking { get; set; }
    public bool CanStrangling { get; set; }
    public bool InitAttack { get; set; }
    public Transform Target { get; private set; }
    private Rigidbody _rb;
    private Animator _animator;
    private SneakSkill _sneakSkill;
    private bool _frozen = false;
    
    [SerializeField] private int minHealth;


    private void Configure(Controller controller, Controller aimController)
    {

        _rb = GetComponent<Rigidbody>();
        _animator = GetComponent<Animator>();
        Target = this.transform.GetChild(0);
        moveController = controller;
        aimMoveController = aimController;
        movementController.Configure(_animator, _rb, Speed);
        _canShoot = aimController.GetComponent<ICanShoot>();
        _sneakSkill = new SneakSkill();
        _sneakSkill.Configure(this, _animator);
        healthController.Configure(minHealth, Health);
    }


    private void Start()
    {
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
            Debug.Log("Hola");
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
