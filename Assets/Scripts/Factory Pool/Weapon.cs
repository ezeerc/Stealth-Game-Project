using System;
using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class Weapon : MonoBehaviour
{
    [SerializeField] private ProjectileObjectPool _prefab;
    private ObjectPool _objectPool;
    protected ObjectPoolFactory _factory;
    public Transform spawnPoint;
    [SerializeField] private float timeBtwShots;
    [SerializeField] protected int qtyBullets;
    private int currentQty;

    public float shotRecharge;    /////////// TOMI //////////////////////////////////

    private void Awake()
    {
        _factory = new ObjectPoolFactory(_prefab);
    }

    /*private void OnEnable()
    {
        _spawnPoint = this.GetComponentInChildren<Transform>().transform;
    }*/

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
            _factory.Create(spawnPoint.transform.position, spawnPoint.transform.rotation);
            yield return new WaitForSeconds(timeBtwShots);
            CoroutineManager.Instance.StartCoroutine(ShotTimeCoroutine());
        }
    }
}