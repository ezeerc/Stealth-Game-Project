using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Shotgun : Weapon
{
    public new void Shot()
    {
        for (int i = 0; i < qtyBullets; i++)
        {
            _factory.Create(spawnPoint.transform.position, spawnPoint.transform.rotation);
        }
    }
}
