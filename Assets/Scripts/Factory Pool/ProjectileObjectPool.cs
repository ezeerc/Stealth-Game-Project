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
    [SerializeField] private LayerMask objectsMask;
    [SerializeField] private float rayDistance = 5f;
    [SerializeField] private LayerMask bulletsMask;
    float detectionRadius = 0.5f;
    internal override void Init()
    {
        rb.velocity = transform.forward * _speed;
        InitializeFixedUpdate = true;
        StartCoroutine(FixedUpdateCoroutine());
        Invoke(nameof(Recycle), 2f);
    }
    

    internal override void Release()
    {

    }

    IEnumerator FixedUpdateCoroutine()
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
                        print("El SphereCollider ha detectado un enemigo.");
                        print("golpeé un enemigo");
                        var damageable = hit.GetComponent<IDamageable>();
                        print("Toqué un enemigo");
                        damageable.TakeDamage(_damage);
                        this.Recycle();
                        this.enabled = false;
                    }
                    else if (hit.gameObject.layer == LayerMask.NameToLayer("Buildings&Props"))
                    {
                        Debug.Log("El SphereCollider ha detectado un edificio.");
                        this.Recycle();
                        this.enabled = false;
                    }
                }
            }
        }
    }
    
    private void OnDrawGizmosSelected()
    {
        Gizmos.color = Color.red;
        Gizmos.DrawWireSphere(transform.position, detectionRadius);
    }
}
