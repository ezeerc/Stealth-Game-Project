using UnityEngine;

public class FleeTargetBehavior : IEnemyBehavior
{
    public void Execute(Enemy enemy)
    {
        if (enemy._player != null)
        {
            enemy.navMeshAgent.isStopped = false;
            enemy.SetRageMode(20f, 20f, 800, 0, true);
            SteeringBehaviors.Flee(enemy.navMeshAgent, enemy._player.transform.position);
        }
    }
}

