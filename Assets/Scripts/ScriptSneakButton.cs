using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ScriptSneakButton : Button
{
    private Button _sneakButton;
    private Player _player;
    private bool _canSneak;
    private Animator _animator;
    private void Start()
    {
        _sneakButton = gameObject.GetComponent<Button>();
        _player = GameObject.FindGameObjectWithTag("Player").GetComponent<Player>();
        _player.OnStrangling += CanSneak;
        _animator = gameObject.GetComponent<Animator>();
    }

    public override void OnClick()
    {
        _player.SneakPosition();
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

    private void OnDestroy()
    {
        _player.OnStrangling -= CanSneak;
    }
}
