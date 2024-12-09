public class Pistol : Weapon
{
    private void Start()
    {
        SetShotStrategy(new SingleShotStrategy());
    }
}