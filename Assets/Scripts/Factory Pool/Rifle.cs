public class Rifle : Weapon
{
    private void Start()
    {
        SetShotStrategy(new BurstShotStrategy());
    }
}
