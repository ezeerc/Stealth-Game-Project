using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyState
{
    public Vector3 Position { get; private set; }
    public int Health { get; private set; }
    public bool IsDead { get; private set; }

    public EnemyState(Vector3 position, int health, bool isDead)
    {
        Position = position;
        Health = health;
        IsDead = isDead;
    }
}