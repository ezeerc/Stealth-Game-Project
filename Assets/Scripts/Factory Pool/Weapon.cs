using System;
using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class Weapon : MonoBehaviour
{
    [SerializeField] private ProjectileObjectPool _prefab;
    private ObjectPoolFactory _factory;
    public Transform spawnPoint;
    [SerializeField] private float timeBtwShots;
    [SerializeField] private int qtyBullets;
    public float shotRecharge;    /////////// TOMI //////////////////////////////////
                                  /// 
    private IShotStrategy _shotStrategy;

    private void Awake()
    {
        _factory = new ObjectPoolFactory(_prefab);
        spawnPoint = GetComponent<Transform>();
    }

    public void SetShotStrategy(IShotStrategy strategy)
    {
        _shotStrategy = strategy;
    }

    public void Shot()
    {
        if (_shotStrategy != null)
        {
            _shotStrategy.Execute(spawnPoint, _factory, qtyBullets, timeBtwShots);
        }
        else
        {
            Debug.LogWarning("No strategy set for shooting.");
        }
    }
}
