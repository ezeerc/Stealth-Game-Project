using System;
using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

[RequireComponent(typeof(Rigidbody))]
public class ProjectileObjectPool : RecyclableObject
{
    [SerializeField] protected Rigidbody rb;
    [SerializeField] private int _speed;
    [SerializeField] private int _damage;
    [SerializeField] private LayerMask bulletsMask;
    [SerializeField] float detectionRadius = 0.5f;
    [SerializeField] float timeToRecycle = 0.5f;
    [SerializeField] private float soundRadius = 10f;
    [SerializeField] private LayerMask enemyLayer;

    internal override void Init()
    {
        rb.velocity = transform.forward * _speed;
        InitializeFixedUpdate = true;
        NotifyNearbyEnemies();
        StartCoroutine(FixedUpdateCoroutine());
        Invoke(nameof(Recycle), timeToRecycle);
    }


    internal override void Release()
    {
    }

    IEnumerator FixedUpdateCoroutine() //implementación de un fixedupdate artificial
    {
        while (InitializeFixedUpdate)
        {
            yield return new WaitForFixedUpdate();

            Collider[] hits = Physics.OverlapSphere(transform.position, detectionRadius, bulletsMask);

            foreach (Collider hit in hits)
            {
                if (hit != null)
                {
                    if (hit.gameObject.layer == LayerMask.NameToLayer("Enemy"))
                    {
                        Enemy enemy = hit.GetComponent<Enemy>();
                        if (enemy != null && enemy.stats != null)
                        {
                            SoldierStats enemyStats = enemy.stats;

                            var damageable = hit.GetComponent<IDamageable>();
                            if (damageable != null && !enemyStats.armour)
                            {
                                damageable.TakeDamage(_damage);
                            }

                            this.Recycle();
                            this.enabled = false;
                        }
                    }
                    else if (hit.gameObject.layer == LayerMask.NameToLayer("Buildings&Props"))
                    {
                        this.Recycle();
                        this.enabled = false;
                    }
                    else if (hit.gameObject.layer == LayerMask.NameToLayer("Player"))
                    {
                        var damageable = hit.GetComponent<IDamageable>();
                        damageable.TakeDamage(_damage);
                        this.Recycle();
                        this.enabled = false;
                    }
                }
            }
        }
    }

    private void NotifyNearbyEnemies()
    {
        Collider[] enemiesInRange = Physics.OverlapSphere(transform.position, soundRadius, enemyLayer);
        foreach (var collider in enemiesInRange)
        {
            var soundObserver = collider.GetComponent<ISoundObserver>();
            if (soundObserver != null)
            {
                soundObserver.OnSoundDetected(transform.position);
            }
        }
    }
    
    /*private void OnDrawGizmosSelected()
    {
        Gizmos.color = Color.red;
        Gizmos.DrawWireSphere(transform.position, detectionRadius);
    }*/
}