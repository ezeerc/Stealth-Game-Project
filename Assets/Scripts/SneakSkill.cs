using UnityEngine;

public class SneakSkill : Skill
{
    private static readonly int Sneak = Animator.StringToHash("Sneak");
    private static readonly int Strangling = Animator.StringToHash("Strangling");
    private static readonly int Run = Animator.StringToHash("Run");
    private Animator _animator;
    private Player _player;


    public void Configure(Player player, Animator animator)
    {
        _player = player;
        _animator = animator;
    }
    public override void GetSkill()
    {
        /*if ((Input.touchCount > 1) && (Input.GetTouch(1).phase == TouchPhase.Began))
        {
            if (!_player.Sneaking)
            {
                _animator.SetBool(Sneak, false);
                _player.ChangeSpeed(10);
                _player.Sneaking = true;
            }
            else
            {
                _animator.SetBool(Sneak, true);
                _player.Sneaking = false;
                _player.ChangeSpeed(5);
            }
        }

        if ((Input.touchCount <= 1) || (Input.GetTouch(1).phase != TouchPhase.Began)) return;*/
        if (!_player.Sneaking || !_player.CanStrangling) return;
        _player.InitAttack = true;
        StealthAttack();
    }

    public void SneakPosition()
    {
        if (_player.Sneaking)
        {
            GetSkill();
            _animator.SetBool(Sneak, false);
            _animator.SetInteger("WeaponType_int", 1);
            _player.ChangeSpeed(10);
            _player.Sneaking = false;
            
            
        }
        else
        {
            _animator.SetBool(Sneak, true);
            _animator.SetInteger("WeaponType_int", 0);
            _player.Sneaking = true;
            _player.ChangeSpeed(5);
        }
    }
    private void StealthAttack()
    {
        if (_player.Sneaking && _player.CanStrangling)
        {
            _player.Sneaking = false;
            _player.CanStrangling = false;
            _animator.SetInteger("WeaponType_int", 0);
            _player.FrozenMove(3);
            _animator.SetBool(Sneak, false);
            _animator.SetBool(Run, false);
            _animator.SetTrigger(Strangling);
        }
    }
}