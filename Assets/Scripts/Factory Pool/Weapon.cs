using System;
using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class Weapon : MonoBehaviour
{
    [SerializeField] private ProjectileObjectPool _prefab;
    private ObjectPool _objectPool;
    private ObjectPoolFactory _factory;
    [SerializeField] private GameObject _spawnPoint;

    private void Awake()
    {
        _factory = new ObjectPoolFactory(_prefab);
        
    }

    public void Shot()
    {
        _factory.Create(_spawnPoint.transform.position, _spawnPoint.transform.rotation);
    }

}