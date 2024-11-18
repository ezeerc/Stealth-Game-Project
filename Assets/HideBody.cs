using System;
using UnityEngine;
using UnityEngine.AI;

public class HideBody : MonoBehaviour
{
    public Player _player;
    private GameObject _enemy;
    public float detectionRadius = 1.5f;
    public LayerMask playerLayer;

    [SerializeField] private Enemy _enemyScript;

    private void Start()
    {
        Player.OnHideMovement += HideBodyMovement;
    }

    private void OnTriggerEnter(Collider other)
    {
        if (!_enemyScript.Dead) return;

        if (other.CompareTag("Player"))
        {
            _player = other.GetComponent<Player>();
            _player.CanHideFunc();
        }
        else if (other.gameObject.layer == LayerMask.NameToLayer("Enemy"))
        {
            print("enemigo encontrado");
        }
    }

    private void OnTriggerExit(Collider other)
    {
        if (!other.CompareTag("Player")) return;
        if (!_enemyScript.Dead) return;
        _player.CanHideFunc();
    }

    private void HideBodyMovement()
    {
        if (_player != null && Vector3.Distance(this.transform.position, _player.transform.position) < 5)
        {
            _enemyScript.HideBody(_player);
        }
    }

    private void OnDestroy()
    {
        Player.OnHideMovement -= HideBodyMovement;
    }
}