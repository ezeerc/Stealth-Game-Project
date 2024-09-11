using UnityEngine;

public class SneakSkill : Skill
{
    private static readonly int Sneak = Animator.StringToHash("Sneak");
    private static readonly int Strangling = Animator.StringToHash("Strangling");
    private static readonly int Run = Animator.StringToHash("Run");
    private readonly Animator _animator;
    private readonly Player _player;
    public bool Sneaking { get; set; }
    public bool CanStrangling { get; set; }
    public bool InitAttack { get; set; }

    public SneakSkill(Player player, Animator animator)
    {
        _player = player;
        _animator = animator;
    }
    public override void GetSkill()
    {
        if ((Input.touchCount > 1) && (Input.GetTouch(1).phase == TouchPhase.Began))
        {
            if (!Sneaking)
            {
                _animator.SetBool(Sneak, false);
                _player.Speed = 10;
                Sneaking = true;
            }
            else
            {
                _animator.SetBool(Sneak, true);
                Sneaking = false;
                _player.Speed = 5;
            }
        }

        if ((Input.touchCount <= 1) || (Input.GetTouch(1).phase != TouchPhase.Began)) return;
        if (!Sneaking || !CanStrangling) return;
        InitAttack = true;
        StealthAttack();
    }

    private void StealthAttack()
    {
        if (Sneaking && CanStrangling)
        {
            _player.FrozenMove(3);
            _animator.SetBool(Sneak, false);
            _animator.SetBool(Run, false);
            _animator.SetTrigger(Strangling);
            Sneaking = false;
            CanStrangling = false;
        }
    }
}