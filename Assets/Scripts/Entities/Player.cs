using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

[RequireComponent(typeof(Rigidbody), typeof(CapsuleCollider))]
public class Player : MonoBehaviour
{
    private static readonly int Run = Animator.StringToHash("Run");
    [SerializeField] private Controller controller;
    [SerializeField] private Controller aimController;
    [field: SerializeField] public float Speed{get;set;}
    public Transform Target { get; private set; }
    private Rigidbody _rb;
    private Animator _animator;
    private PlayerMovement _playerMovement;
    public SneakSkill Sneak { get; set; }
    private bool _frozen = false;


    private void Start()
    {
        Target = this.transform.GetChild(0);
        _rb = GetComponent<Rigidbody>();
        _animator = GetComponent<Animator>();
        Sneak = new SneakSkill(this, _animator);
        _playerMovement = new PlayerMovement(this);
    }

    private void FixedUpdate()
    {
        if(_frozen) return;
        _playerMovement.PlayerMovementMethod(_rb, controller, Speed, _animator);
    }

    private void Update()
    {
        Sneak.GetSkill();
        _playerMovement.PlayerAimMethod(_rb, aimController, Speed, _animator);
    }

    public void FrozenMove(int time)
    {
        StartCoroutine(FrozenCoroutine(time));
    }
    
    private IEnumerator FrozenCoroutine(int time)
    {
        _frozen = true;
        yield return new WaitForSeconds(time);
        _frozen = false;
    }
}

