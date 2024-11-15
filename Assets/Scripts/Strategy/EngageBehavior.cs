public class EngageBehavior : IEnemyBehavior
{
    public void Execute(Enemy enemy)
    {
        enemy.SetRageMode(6f, 6f, 360, 15, false);
        SteeringBehaviors.Seek(enemy.navMeshAgent, enemy._player.transform.position);
        enemy.transform.LookAt(enemy._player.transform.position);

        if (enemy.canShoot)
        {
            CoroutineManager.Instance.StartCoroutine(enemy.ShootCoroutine(enemy.timeBetweenAttacks));
        }
    }
}

