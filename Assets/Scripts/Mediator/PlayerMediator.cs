using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerMediator : MonoBehaviour, IPlayer
{
    [SerializeField] MovementController movementController;
    [SerializeField] WeaponController weaponController;
    
    [SerializeField] private Controller moveController;
    [SerializeField] private Controller aimMoveController;
    

    private ICanShoot _canShoot;
    public bool Sneaking { get; set; }
    public bool CanStrangling { get; set; }
    public bool InitAttack { get; set; }
    public Transform Target { get; private set; }
    private Rigidbody _rb;
    private Animator _animator;
    private SneakSkill _sneakSkill;
    private bool _frozen = false;

    public void Configure(Controller controller, Controller aimController)
    {
        _rb = GetComponent<Rigidbody>();
        _animator = GetComponent<Animator>();
        Target = this.transform.GetChild(0);
        moveController = controller;
        aimMoveController = aimController;
        movementController.Configure(_animator, _rb);
        _canShoot = aimController.GetComponent<ICanShoot>();
        _sneakSkill = new SneakSkill();
        _sneakSkill.Configure(this, _animator);
    }


    private void Start()
    {
        this.Configure(moveController, aimMoveController);
    }

    private void FixedUpdate()
    {
        if(_frozen) return;
        var direction = moveController.GetMovementInput();
        movementController.Move(direction);
    }

    private void Update()
    {
        _sneakSkill.GetSkill();
        var direction = aimMoveController.GetMovementInput();
        movementController.MoveAim(direction);
        CheckAimMovement(aimMoveController);
        Shot(_canShoot);
        
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
    
}
