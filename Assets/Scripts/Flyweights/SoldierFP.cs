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
        armour = false,
        fireRate = 2,
    };

    public static readonly Flyweight assaultSoldier = new Flyweight()
    {
        baseSpeed = 5,
        runSpeed = 10,
        damage = 1,
        armour = false,
        fireRate = 5,
    };

    public static readonly Flyweight armouredSoldier = new Flyweight()
    {
        baseSpeed = 4,
        runSpeed = 7,
        damage = 1,
        armour = true,
        fireRate = 10,
    };
}
