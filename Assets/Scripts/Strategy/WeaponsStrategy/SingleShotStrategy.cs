using UnityEngine;

public class SingleShotStrategy : IShotStrategy
{
    public void Execute(Transform spawnPoint, ObjectPoolFactory factory, int qtyBullets, float timeBtwShots)
    {
        factory.Create(spawnPoint.position, spawnPoint.rotation);
    }
}
