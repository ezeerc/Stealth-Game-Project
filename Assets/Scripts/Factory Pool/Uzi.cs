using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Uzi :Weapon
{
    private void Start()
    {
        SetShotStrategy(new BurstShotStrategy());
    }

}
