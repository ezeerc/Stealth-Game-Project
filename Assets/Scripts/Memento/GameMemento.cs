using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameMemento
{
    public Vector3 PlayerPosition { get; private set; }
    public int PlayerHealth { get; private set; }
    public List<EnemyState> EnemiesState { get; private set; }

    public GameMemento(Vector3 playerPosition, int playerHealth, List<EnemyState> enemiesState)
    {
        PlayerPosition = playerPosition;
        PlayerHealth = playerHealth;
        EnemiesState = new List<EnemyState>(enemiesState);
    }
}
