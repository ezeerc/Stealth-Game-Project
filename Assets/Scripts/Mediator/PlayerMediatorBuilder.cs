using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerBuilder : MonoBehaviour
{
    private Player _prefab;
    private Rigidbody _rigidbody;
    private Animator _animator;
    private Controller _moveController;
    private Controller _aimMoveController;
    public Transform Target { get; private set; }
    
    public PlayerBuilder FromPrefab(Player prefab)
    {
        _prefab = prefab;
        return this;
    }

    public PlayerBuilder WithRigidBody(Rigidbody rigidBody)
    {
        _rigidbody = rigidBody;
        return this;
    }

    public PlayerBuilder WithTargetPoint(Transform targetPoint)
    {
        Target = targetPoint;
        return this;
    }

    public PlayerBuilder WithMoveController(Controller moveController)
    {
        _moveController = moveController;
        return this;
    }
    
    public PlayerBuilder WithAimMoveController(Controller aimMoveController)
    {
        _aimMoveController = aimMoveController;
        return this;
    }
    
    

    /*movementController.Configure(_animator, _rb, speed);
    _canShoot = aimController.GetComponent<ICanShoot>();
    _sneakSkill = new SneakSkill();
    _sneakSkill.Configure(this, _animator);
    healthController.Configure(minHealth, maxHealth);*/
    
    
}
