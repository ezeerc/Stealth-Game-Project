using UnityEngine;


public class PatrolBehavior : IEnemyBehavior
{
    public void Execute(Enemy enemy)
    {
        enemy.SetRageMode(2f, 5f, 120, 0, true);
        if (enemy.waypoints.Length == 0) return;

        Transform targetWaypoint = enemy.waypoints[enemy.currentWaypointIndex];
        SteeringBehaviors.Seek(enemy.navMeshAgent, targetWaypoint.position);

        if (Vector3.Distance(enemy.transform.position, targetWaypoint.position) <= enemy.distanceToFollowPath)
        {
            enemy.currentWaypointIndex = (enemy.currentWaypointIndex + 1) % enemy.waypoints.Length;
        }
    }
}