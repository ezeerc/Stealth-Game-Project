public class FleeBehavior : IEnemyBehavior
{
    public void Execute(Enemy enemy)
    {
        enemy.SetRageMode(30f, 30f, 800, 0, true);
        SteeringBehaviors.Flee(enemy.navMeshAgent, enemy._player.transform.position);
    }
}

