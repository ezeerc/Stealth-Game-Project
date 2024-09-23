using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(Rigidbody))]
public class ProjectileObjectPool : RecyclableObject
{
    [SerializeField] protected Rigidbody rb;
    [SerializeField] private int _speed;
    [SerializeField] private int _damage;
    internal override void Init()
    {
        rb.velocity = transform.forward * _speed;
        Invoke(nameof(Recycle), 2f);
    }
    internal override void Release()
    {

    }

    private void OnTriggerEnter(Collider other)
    {
        var damageable = other.GetComponent<IDamageable>();
        if (damageable != null)
        {
            damageable.TakeDamage(_damage);
        }
    }
}
