using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MenuAnims : MonoBehaviour
{
    private Animator _anim;
    public AnimationState animationState;
    public IdleState idleState;

    private void Awake()
    {
        _anim = GetComponent<Animator>();
    }

    private void Start()
    {
        float randomSpeed = Random.Range(0.2f, 1f);


        switch (idleState)
        {
            case IdleState.Leaning:
                _anim.SetInteger("Animation_int", 8);
                break;
            case IdleState.WipeMouth:
                _anim.SetInteger("Animation_int", 7);
                break;
            case IdleState.Sitting:
                _anim.SetInteger("Animation_int", 9);
                break;
            case IdleState.Smoking:
                randomSpeed = Random.Range(0.01f, 0.7f);
                _anim.SetInteger("Animation_int", 5);
                break;
            case IdleState.CheckWatch:
                randomSpeed = 0.4f;
                _anim.SetInteger("Animation_int", 3);
                break;
        }


        switch (animationState)
        {
            case AnimationState.Crouching:
                _anim.SetBool("Crouch_b", true);
                break;
            case AnimationState.Standing:
                break;
            case AnimationState.RifleIdle:
                _anim.SetInteger("WeaponType_int", 3);
                break;
            case AnimationState.NoWeapon:
                _anim.SetInteger("WeaponType_int", 0);
                break;
        }
 
        _anim.speed = randomSpeed;
    }


    public enum AnimationState
    {
        Crouching,
        Standing,
        RifleIdle,
        NoWeapon
    }

    public enum IdleState
    {
        Leaning,
        WipeMouth,
        Sitting,
        Smoking,
        CheckWatch
    }
}
