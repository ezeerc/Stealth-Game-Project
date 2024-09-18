using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UIElements;

public class ObjectPoolFactory 
{
    private readonly ProjectileObjectPool _prefab;
    private readonly ObjectPool _objectPool;

    public ObjectPoolFactory(ProjectileObjectPool prefab)
    {
        _prefab = prefab;
        _objectPool = new ObjectPool(_prefab);
        _objectPool.Init(10);
    }

    public ProjectileObjectPool Create(Vector3 position, Quaternion rotation)
    {
        return _objectPool.Spawn<ProjectileObjectPool>(position, rotation);
    }
}
