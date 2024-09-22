using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WeaponController : MonoBehaviour
{
    [SerializeField] private ProjectileObjectPool prefab;
    private ObjectPool _objectPool;
    private ObjectPoolFactory _factory;
    [SerializeField] private Transform spawnPoint;
    [SerializeField] private LaserScript laser;
    public bool LaserOn = false;
    private void Awake()
    {
        _factory = new ObjectPoolFactory(prefab);
    }

    public void Shot()
    {
        _factory.Create(spawnPoint.transform.position, spawnPoint.transform.rotation);
    }

    private void Update()
    {
        if (LaserOn)
        {
            laser.LaserOn();
        }
        else if(!LaserOn)
        {
            laser.LaserOff();
        }
    }
}
