using System;
using UnityEngine;

public class PlayerMovement
{
    private static readonly int Run = Animator.StringToHash("Run");
    private static readonly int Aiming = Animator.StringToHash("Aiming");
    private static readonly int Sneak = Animator.StringToHash("Sneak");
    private static readonly int Direction = Animator.StringToHash("direction");
    private readonly Player _player;
    private bool _notMoving;
    private bool _rotating;
        
    public PlayerMovement(Player player)
    {
        _player = player;
    }

    public void PlayerMovementMethod(Rigidbody rigidbody, Controller controller, float speed, Animator animator)
    {
        if (_notMoving) return;
        var directionFix = new Vector3(controller.GetMovementInput().x * speed, rigidbody.velocity.y,
            controller.GetMovementInput().z * speed);
        rigidbody.velocity = directionFix;
        if (controller.GetMovementInput().x != 0f || controller.GetMovementInput().y != 0f)
        {
            animator.SetBool(Run, true);
            animator.SetFloat(Direction, controller.GetMovementInput().x);
            if (_rotating) return;
            _player.transform.rotation = Quaternion.LookRotation(rigidbody.velocity);
        }
        else
        {
            animator.SetBool(Run, false);
        }
    }

    public void PlayerAimMethod(Rigidbody rigidbody, Controller controller, float speed, Animator animator)
    {
        var directionFix = new Vector3(controller.GetMovementInput().x, 0, controller.GetMovementInput().z);
        Vector3 lookAtPoint = _player.transform.position + directionFix;
        _player.transform.LookAt(lookAtPoint);
        if (controller.GetMovementInput().x != 0f || controller.GetMovementInput().y != 0f)
        {
            _rotating = true;
            animator.SetBool(Aiming, true);
            animator.SetBool(Sneak, false);

        }
        else
        {
            animator.SetBool(Aiming, false);
            _rotating = false;
        }
    }
}