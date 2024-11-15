using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MoveToTargetBehavior : IEnemyBehavior
{
    private Vector3 _targetEnd;

    public MoveToTargetBehavior(Vector3 targetEnd)
    {
        _targetEnd = targetEnd;
    }

    public void Execute(Enemy enemy)
    {
        if (enemy.navMeshAgent != null)
        {
            enemy.navMeshAgent.isStopped = false;
            enemy.navMeshAgent.SetDestination(_targetEnd);
        }
    }
}

