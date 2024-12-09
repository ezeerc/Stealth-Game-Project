using UnityEngine;

public interface IShotStrategy
{
    void Execute(Transform spawnPoint, ObjectPoolFactory factory, int qtyBullets, float timeBtwShots);
}

