using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class InvestigateBehavior : IEnemyBehavior
{
    public void Execute(Enemy enemy)
    {
        if (enemy.navMeshAgent.remainingDistance <= enemy.navMeshAgent.stoppingDistance)
        {
            Debug.Log("estoy investigando el área");
            enemy.SetBehavior(new PatrolBehavior());
        }
    }
}
