public class IdleTargetBehavior : IEnemyBehavior
{
    public void Execute(Enemy enemy)
    {
        enemy.navMeshAgent.isStopped = true;
    }
}
