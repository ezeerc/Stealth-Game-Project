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
    private AudioSource _source;
    [SerializeField] private AudioClip _getClip;

    private IShotStrategy _shotStrategy;

    private void Awake()
    {
        _factory = new ObjectPoolFactory(_prefab);
        spawnPoint = GetComponent<Transform>();
        _source = GetComponent<AudioSource>();
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
            GetSfx();
        }
        else
        {
            Debug.LogWarning("No se asignó audio para reproducir");
        }
    }
    
    public void GetSfx(AudioClip customClip = null)
    {
        AudioClip clipToPlay = customClip ?? _getClip;

        if (clipToPlay == null)
        {
            Debug.LogWarning("No se asignó audio para reproducir");
            return;
        }
        
        if (_source.clip != clipToPlay || !_source.isPlaying)
        {
            if (_source.clip != null)
            {
                _source.Stop();
            }
            _source.clip = clipToPlay;
            _source.Play();
        }
    }

}
