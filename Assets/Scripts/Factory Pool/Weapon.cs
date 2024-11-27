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
    private AudioSource _source;
    [SerializeField] private AudioClip _getClip;

    private void Awake()
    {
        _factory = new ObjectPoolFactory(_prefab);
        spawnPoint = GetComponent<Transform>();
        _source = GetComponent<AudioSource>();
    }

    public void Shot()
    {
        currentQty = 0;
        CoroutineManager.Instance.StartCoroutine(ShotTimeCoroutine());
        GetSfx();
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
            _source.clip = clipToPlay;
            _source.Play();
        }
    }
}