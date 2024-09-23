using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public static class SoldierFP
{
    public static readonly Flyweight pistolSoldier = new Flyweight()
    {
        baseSpeed = 5,
        runSpeed = 12,
        damage = 1,
        armour = 1,
        fireRate = 2,
    };

    public static readonly Flyweight assaultSoldier = new Flyweight()
    {
        baseSpeed = 5,
        runSpeed = 10,
        damage = 1,
        armour = 1,
        fireRate = 5,
    };

    public static readonly Flyweight machineGunnerSoldier = new Flyweight()
    {
        baseSpeed = 5,
        runSpeed = 7,
        damage = 1,
        armour = 1,
        fireRate = 10,
    };
}
