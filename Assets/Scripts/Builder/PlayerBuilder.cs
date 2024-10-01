using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerBuilder
{
    private Controller _moveController;
    private Controller _aimMoveController;
    private MovementController _movementController;
    private WeaponController _weaponController;
    private HealthController _healthController;
    private int _minHealth;
    private int _maxHealth;
    private Transform _target;
    private Vector3 _instantiationPoint = new Vector3(0.35f, 0.04f, -4.35f);
    private int _speed;
    private LayerMask _playerMask;
    private SneakSkill _sneakSkill;

    public PlayerBuilder SetMoveController(Controller moveController)
    {
        _moveController = moveController;
        return this;
    }

    public PlayerBuilder SetAimMoveController(Controller aimMoveController)
    {
        _aimMoveController = aimMoveController;
        return this;
    }

    public PlayerBuilder SetMinHealth(int minHealth)
    {
        _minHealth = minHealth;
        return this;
    }
    
    public PlayerBuilder SetMaxHealth(int maxHealth)
    {
        _maxHealth = maxHealth;
        return this;
    }
    
    public PlayerBuilder SetPosition(Vector3 instantiationPoint)
    {
        _instantiationPoint = instantiationPoint;
        return this;
    }
    
    public PlayerBuilder SetSpeed(int speed)
    {
        _speed = speed;
        return this;
    }

    public PlayerBuilder SetLayerMask(LayerMask playerMask)
    {
        _playerMask = playerMask;
        return this;
    }
    
    public PlayerBuilder SetSneakSkill(SneakSkill sneakSkill)
    {
        _sneakSkill = sneakSkill;
        return this;
    }
    public Player Build(Player player)
    {
        player.moveController = _moveController;
        player.aimMoveController = _aimMoveController;
        player.minHealth = _minHealth;
        player.Target = _target;
        player.transform.position = _instantiationPoint;
        player.Speed = _speed;
        player.playerMask = _playerMask;
        player.sneakSkill = _sneakSkill;
        return player;
    }
}
