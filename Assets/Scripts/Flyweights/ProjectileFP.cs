using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ProjectileFP : MonoBehaviour
{
    public static readonly Flyweight bullet = new Flyweight()
    {
        baseSpeed = 5,
        damage = 1
    };
}
