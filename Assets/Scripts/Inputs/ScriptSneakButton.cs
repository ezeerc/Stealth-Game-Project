using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ScriptSneakButton : Button
{
    private Button _sneakButton;
    private Player _player;
    private bool _canSneak;
    private bool _canHide;
    private Animator _animator;
    private void Start()
    {
        _sneakButton = gameObject.GetComponent<Button>();
        _player = GameObject.FindGameObjectWithTag("Player").GetComponent<Player>();
        _player.OnStrangling += CanSneak;
        _player.OnHiding += CanHide;
        _animator = gameObject.GetComponent<Animator>();
    }

    public override void OnClick()
    {
        _player.StealthAttack();
        _player.HideMovement();
    }

    public void CanSneak()
    {
        _canSneak = !_canSneak;
        if (_canSneak)
        {
            _animator.SetBool("SelectedBool", true);
        }
        else
        {
            _animator.SetBool("SelectedBool", false);
        }
        
    }
    
    public void CanHide()
    {
        _canHide = !_canHide;
        if (_canHide)
        {
            _animator.SetBool("Hiding", true);
        }
        else
        {
            _animator.SetBool("Hiding", false);
        }
        
    }

    private void OnDestroy()
    {
        _player.OnStrangling -= CanSneak;
        _player.OnHiding -= CanHide;
    }
}
