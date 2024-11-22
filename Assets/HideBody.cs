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
            _player.OnHide(true);
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
        _player.OnHide(false);
    }

    private void HideBodyMovement()
    {
        if (_player != null && Vector3.Distance(this.transform.position, _player.transform.position) < 2)
        {
            _enemyScript.HideBody(_player);
            _player.OnHide(false);
        }
    }

    private void OnDestroy()
    {
        Player.OnHideMovement -= HideBodyMovement;
    }
}