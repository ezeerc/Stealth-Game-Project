using UnityEngine;

public class PatrolBehavior : IEnemyBehavior
{
    private bool _initialRotationSet = false;
    private Quaternion _initialRotation;

    public void Execute(Enemy enemy)
    {
        enemy.SetRageMode(2f, 5f, 120, 0, true);
        
        if (enemy.waypoints.Length == 0) return;
        
        if (!_initialRotationSet && enemy.waypoints.Length == 1)
        {
            _initialRotation = enemy.initialRotation;
            _initialRotationSet = true;
        }
        
        Transform targetWaypoint = enemy.waypoints[enemy.currentWaypointIndex];
        SteeringBehaviors.Seek(enemy.navMeshAgent, targetWaypoint.position);
        
        if (Vector3.Distance(enemy.transform.position, targetWaypoint.position) <= enemy.distanceToFollowPath)
        {
            if (enemy.waypoints.Length > 1)
            {
                enemy.currentWaypointIndex = (enemy.currentWaypointIndex + 1) % enemy.waypoints.Length;
            }
            else
            {
                enemy.transform.rotation = Quaternion.Slerp(enemy.transform.rotation, _initialRotation, Time.deltaTime * 2f);
            }
        }
    }
}