using UnityEngine;

public class PlayerMovement
{
    private static readonly int Run = Animator.StringToHash("Run");
    private static readonly int Aiming = Animator.StringToHash("Aiming");
    private readonly Player _player;
    private bool _notMoving;

    public PlayerMovement(Player player)
    {
        _player = player;
    }

    public void PlayerMovementMethod(Rigidbody rigidbody, Controller controller, float speed, Animator animator)
    {
        if (_notMoving) return;
        var directionFix = new Vector3(controller.GetMovementInput().x * speed, rigidbody.velocity.y, controller.GetMovementInput().z * speed);
        rigidbody.velocity = directionFix;
        if (controller.GetMovementInput().x != 0f || controller.GetMovementInput().y != 0f)
        {
            _player.transform.rotation = Quaternion.LookRotation(rigidbody.velocity);
            animator.SetBool( Run, true);
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
            _notMoving = true;
            animator.SetBool(Aiming, true);
        }
        else
        {
            animator.SetBool(Aiming, false);
            _notMoving = false;
        }
    }
    
}