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
        _player.OnHidingOn += CanHide;
        _player.OnHidingOff += CantHide;
        _animator = gameObject.GetComponent<Animator>();
    }

    private void Update() // SOLO PARA TESTEAR EN PC TOMI
    {
        if (Input.GetKey(KeyCode.Space))
        {
            _player.StealthAttack();
            _player.HideMovement();
        }
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
            _animator.SetBool("Hiding", true);
    }
    
    public void CantHide()
    {
            _animator.SetBool("Hiding", false);
    }

    private void OnDestroy()
    {
        _player.OnStrangling -= CanSneak;
        _player.OnHidingOn -= CanHide;
    }
}
