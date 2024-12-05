using System;
using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.SceneManagement;

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
    private const string EquippedKey = "Equipped";


    public float shotCooldown = 1f; //////////////// TOMI //////////////////////////////////
    public bool isShotReady = true; ////////////// TOMI //////////////////////////////////

    public event Action<int> OnWeaponChanged;
    private void Awake()
    {
        _factory = new ObjectPoolFactory(prefab);
    }

    private void Start()
    {
        string activeSceneName = SceneManager.GetActiveScene().name;
        LoadEquippedWeapon();
        if (activeSceneName != "Level_Tutorial")
        {
            ChangeWeapon(weaponActive);
        }
        else
        {
            ChangeWeapon(6);
        }
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

        LaserColor();
    }

    public void Shot()
    {
        if (!isShotReady) return;
        
        weaponActiveScript.Shot();
        isShotReady = false;
        shotCooldown = 0f;

    }

    private void LoadEquippedWeapon()
    {
        string equippedWeaponID = PlayerPrefs.GetString(EquippedKey, string.Empty);

        switch (equippedWeaponID)
        {
            case "Item_0":
                weaponActive = 0;
                break;
            case "Item_1": 
                weaponActive = 1;
                break;
            case "Item_2": 
                weaponActive = 2;
                break;
            case "Item_3": 
                weaponActive = 3;
                break;
            case "Item_4": 
                weaponActive = 4;
                break;
            default:
                weaponActive = 5;
                break;
        }
    }

    private void LaserColor()
    {
        if (isShotReady)
        {
            if (!laser._lineRenderer) return;
            laser._lineRenderer.material.SetColor("_Color", Color.green);
        }
        else
        {
            if (!laser._lineRenderer) return;
            laser._lineRenderer.material.SetColor("_Color", Color.red);
        }
    }
    public void ChangeWeapon(int number)
    {
        for (int i = 0; i < weaponPrefabs.Length; i++)
        {
            weaponPrefabs[i].SetActive(false);
        }

        if (number != 6)
        {
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
        else
        {
            weaponPrefabs[6].SetActive(true);
        }
        OnWeaponChanged?.Invoke(number);
    }
}