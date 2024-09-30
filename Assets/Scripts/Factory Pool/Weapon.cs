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
    [SerializeField] private Transform _spawnPoint;
    [SerializeField] private float timeBtwShots;
    [SerializeField] private int qtyBullets;
    private int currentQty;

    private void Awake()
    {
        _factory = new ObjectPoolFactory(_prefab);
    }

    private void OnEnable()
    {
        _spawnPoint = this.GetComponentInChildren<Transform>().transform;
    }

    public void Shot()
    {
        currentQty = 0;
        CoroutineManager.Instance.StartCoroutine(ShotTimeCoroutine());
    }

    IEnumerator ShotTimeCoroutine()
    {
        if (currentQty < qtyBullets)
        {
            currentQty++;
            _factory.Create(_spawnPoint.transform.position, _spawnPoint.transform.rotation);
            yield return new WaitForSeconds(timeBtwShots);
            CoroutineManager.Instance.StartCoroutine(ShotTimeCoroutine());
        }
    }
}