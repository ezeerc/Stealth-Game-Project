using System;
using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class WeaponController : MonoBehaviour
{
    [SerializeField] private ProjectileObjectPool prefab;
    private ObjectPool _objectPool;
    private ObjectPoolFactory _factory;
    [SerializeField] private Transform spawnPoint;
    [SerializeField] private LaserScript laser;
    [SerializeField] private GameObject[] weaponPrefabs;
    [SerializeField] private int weaponActive;
    public bool LaserOn = false;
    public Weapon weaponActiveScript; // ver si podemos dejar esto privado


    public float shotCooldown = 1f; //////////////// TOMI //////////////////////////////////
    public bool isShotReady = true; ////////////// TOMI //////////////////////////////////

    private void Awake()
    {
        _factory = new ObjectPoolFactory(prefab);
    }

    private void Start()
    {
        ChangeWeapon(weaponActive);
    }

    private void Update()
    {
        if (LaserOn)
        {
            laser.LaserOn();
        }
        else if (!LaserOn)
        {
            laser.LaserOff();
        }
    }

    public void Shot()
    {
        if (!isShotReady) return;

        weaponActiveScript.Shot();
        isShotReady = false;
        shotCooldown = 0f;
    }


    public void ChangeWeapon(int number)
    {
        for (int i = 0; i < weaponPrefabs.Length; i++)
        {
            weaponPrefabs[i].SetActive(false);
        }

        for (int i = 0; i < weaponPrefabs.Length; i++)
        {
            if (number == i)
            {
                weaponPrefabs[i].SetActive(true);
                spawnPoint = weaponPrefabs[i].GetComponentInChildren<Transform>().transform;
                laser = weaponPrefabs[i].GetComponentInChildren<LaserScript>();
                weaponActiveScript = weaponPrefabs[i].GetComponentInChildren<Weapon>();
            }
        }
    }
}