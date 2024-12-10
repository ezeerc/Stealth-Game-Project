using UnityEngine;
using System.Collections;

public class InvestigateDeadBodyBehavior : IEnemyBehavior
{
    private Vector3 _deadBodyPosition;
    private bool _investigationComplete = false;

    public InvestigateDeadBodyBehavior(Vector3 deadBodyPosition)
    {
        _deadBodyPosition = deadBodyPosition;
    }

    public void Execute(Enemy enemy)
    {
        if (_investigationComplete)
        {
            enemy.SetBehavior(new PatrolBehavior());
            return;
        }

        enemy.navMeshAgent.isStopped = false;
        enemy.navMeshAgent.SetDestination(_deadBodyPosition);

        if (!enemy.navMeshAgent.pathPending &&
            enemy.navMeshAgent.remainingDistance <= enemy.navMeshAgent.stoppingDistance)
        {
            enemy.StartCoroutine(InvestigateRoutine(enemy));
        }
    }

    private IEnumerator InvestigateRoutine(Enemy enemy)
    {
        enemy.navMeshAgent.isStopped = true;
        
        enemy.ChangeDetectionStateForTime(10);
        
        yield return RotateByAngle(enemy, 45f);
        
        yield return RotateByAngle(enemy, -90f);
        
        yield return RotateByAngle(enemy, 45f);

        _investigationComplete = true;
        enemy.navMeshAgent.isStopped = false;
        enemy.SetBehavior(new PatrolBehavior());
    }

    private IEnumerator RotateByAngle(Enemy enemy, float angle)
    {
        Quaternion initialRotation = enemy.transform.rotation;
        Quaternion targetRotation = initialRotation * Quaternion.Euler(0, angle, 0);
        float elapsedTime = 0f;
        float rotationDuration = 0.5f;

        while (elapsedTime < rotationDuration)
        {
            enemy.transform.rotation = Quaternion.Slerp(initialRotation, targetRotation, elapsedTime / rotationDuration);
            elapsedTime += Time.deltaTime;
            yield return null;
        }

        enemy.transform.rotation = targetRotation;
    }
}

