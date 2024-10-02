using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyWeaponController : MonoBehaviour
{
    private ObjectPool _objectPool;
    [SerializeField] private Transform spawnPoint;
    [SerializeField] private GameObject[] weaponPrefabs;
    [SerializeField] private int weaponActive;
    [SerializeField] private Weapon weaponActiveScript;
    
    private void Start()
    {
        ChangeWeapon(weaponActive);
    }

    public void Shot()
    {
        weaponActiveScript.Shot();
    }
    private void ChangeWeapon(int number)
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
                weaponActiveScript = weaponPrefabs[i].GetComponentInChildren<Weapon>();
                spawnPoint = weaponActiveScript.spawnPoint;
            }
        }
    }
}
