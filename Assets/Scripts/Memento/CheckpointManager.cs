using System.Collections.Generic;
using UnityEngine;

public class CheckpointManager
{
    public GameMemento _playerMemento;
    public List<EnemyState> _enemiesMemento = new List<EnemyState>();

    public void SaveCheckpoint(Player player, List<Enemy> enemies)
    {
        _playerMemento = player.SaveState(_enemiesMemento);
        
        _enemiesMemento.Clear();
        foreach (var enemy in enemies)
        {
            _enemiesMemento.Add(enemy.SaveState());
        }
        Debug.Log("Checkpoint saved");
    }

    public void LoadCheckpoint(Player player, List<Enemy> enemies)
    {
        if (_playerMemento != null)
        {
            player.RestoreState(_playerMemento);
        }
        
        for (int i = 0; i < enemies.Count; i++)
        {
            if (i < _enemiesMemento.Count)
            {
                enemies[i].RestoreState(_enemiesMemento[i]);
            }
        }
        
        Debug.Log("Checkpoint loaded");
    }
}