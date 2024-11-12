using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class RecyclableObject : MonoBehaviour
{
    private ObjectPool _objectPool;
    protected bool InitializeFixedUpdate;

    internal void Configure(ObjectPool objectPool)
    {
        _objectPool = objectPool;
    }

    public void Recycle()
    {
        InitializeFixedUpdate = false;
        _objectPool.RecycleGameObject(this);
    }

    internal abstract void Init();
    internal abstract void Release();
}
