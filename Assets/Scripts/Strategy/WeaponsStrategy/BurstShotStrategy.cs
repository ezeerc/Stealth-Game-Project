using System.Collections;
using UnityEngine;

public class BurstShotStrategy : IShotStrategy
{
    public void Execute(Transform spawnPoint, ObjectPoolFactory factory, int qtyBullets, float timeBtwShots)
    {
        CoroutineManager.Instance.StartCoroutine(BurstShotCoroutine(spawnPoint, factory, qtyBullets, timeBtwShots));
    }

    private IEnumerator BurstShotCoroutine(Transform spawnPoint, ObjectPoolFactory factory, int qtyBullets, float timeBtwShots)
    {
        for (int i = 0; i < qtyBullets; i++)
        {
            factory.Create(spawnPoint.position, spawnPoint.rotation);
            yield return new WaitForSeconds(timeBtwShots);
        }
    }
}
