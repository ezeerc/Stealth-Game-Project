using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

public class MovementController : MonoBehaviour
{
    private static readonly int Run = Animator.StringToHash("Run");
    private static readonly int Aiming = Animator.StringToHash("Aiming");
    private static readonly int Sneak = Animator.StringToHash("Sneak");
    private static readonly int Direction = Animator.StringToHash("direction");
    private IPlayer _player;
    private bool _notMoving;
    private bool _rotating;
    private Animator _animator;
    private Rigidbody _rigidbody;
    
    [field: SerializeField] public int Speed {get; set;}
    [SerializeField] private Transform _myTransform;

    public void Configure(Animator animator, Rigidbody rigidbody, int speed)
    {
        Speed = speed;
        _animator = animator;
        _rigidbody = rigidbody;
    }
    
    public void Move(Vector3 direction)
    {
        if (_notMoving) return;
        var directionFix = new Vector3(direction.x * Speed, _rigidbody.velocity.y,
            direction.z * Speed);
        _rigidbody.velocity = directionFix;
        if (direction.x != 0f || direction.y != 0f)
        {
            _animator.SetBool(Run, true);
            _animator.SetFloat(Direction, direction.x);
            if (_rotating) return;
            _myTransform.rotation = Quaternion.LookRotation(_rigidbody.velocity);
        }
        else
        {
            _animator.SetBool(Run, false);
        }
    }

    public void MoveAim(Vector3 direction)
    {
        var directionFix = new Vector3(direction.x, 0, direction.z);
        Vector3 lookAtPoint = _myTransform.position + directionFix;
        _myTransform.LookAt(lookAtPoint);
        if (direction.x != 0f || direction.y != 0f)
        {
            _rotating = true;
            _animator.SetBool(Aiming, true);
            _animator.SetBool(Sneak, false);

        }
        else
        {
            _animator.SetBool(Aiming, false);
            _rotating = false;
        }
    }
}
