public class FleeBehavior : IEnemyBehavior
{
    public void Execute(Enemy enemy)
    {
        enemy.SetRageMode(10f, 10f, 600, 0, true);
        SteeringBehaviors.Flee(enemy.navMeshAgent, enemy._player.transform.position);
    }
}

