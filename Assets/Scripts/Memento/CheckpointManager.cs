using System.Collections.Generic;
using UnityEngine;

public class CheckpointManager
{
    public GameMemento _playerMemento;
    public List<EnemyState> _enemiesMemento = new List<EnemyState>();

    // Guarda el estado del jugador y los enemigos
    public void SaveCheckpoint(Player player, List<Enemy> enemies)
    {
        _playerMemento = player.SaveState(_enemiesMemento);

        _enemiesMemento.Clear();
        foreach (var enemy in enemies)
        {
            _enemiesMemento.Add(enemy.SaveState());
        }
        Debug.Log("Checkpoint saved (Player and Enemies)");
    }

    // Sobrecarga: guarda solo el estado del jugador
    public void SaveCheckpoint(Player player)
    {
        _playerMemento = player.SaveState(_enemiesMemento);
        Debug.Log("Checkpoint saved (Player only)");
    }

    // Carga el estado del jugador y los enemigos
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
                enemies[i].transform.position = enemies[i].initialPosition; // Cargar posición original
            }
        }

        Debug.Log("Checkpoint loaded (Player and Enemies)");
    }

    // Sobrecarga: carga solo el estado del jugador
    public void LoadCheckpoint(Player player)
    {
        if (_playerMemento != null)
        {
            player.RestoreState(_playerMemento);
            Debug.Log("Checkpoint loaded (Player only)");
        }
        else
        {
            Debug.LogWarning("No player checkpoint to load!");
        }
    }
}
