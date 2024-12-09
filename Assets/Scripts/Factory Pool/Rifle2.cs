using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Rifle2 : Weapon
{
    private void Start()
    {
        SetShotStrategy(new BurstShotStrategy());
    }

}
