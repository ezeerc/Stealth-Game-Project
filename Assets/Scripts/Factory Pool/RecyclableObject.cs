using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class RecyclableObject : MonoBehaviour
{
    private ObjectPool _objectPool;
    protected bool InitializeFixedUpdate;
    //[SerializeField] private GameObject _objectPrefab;

    internal void Configure(ObjectPool objectPool)
    {
        _objectPool = objectPool;
    }

    public void Recycle()
    {
        InitializeFixedUpdate = false;
        _objectPool.RecycleGameObject(this);
        /*if (_objectPrefab != null) //sirve para chequear cuán lejos debe ir el laser
        {
            Instantiate(_objectPrefab, transform.position, Quaternion.identity);
        }*/
    }

    internal abstract void Init();
    internal abstract void Release();
}
