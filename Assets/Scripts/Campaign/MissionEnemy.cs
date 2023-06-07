[System.Serializable]
public class MissionEnemy
{
    public HeroName heroName = HeroName.Peasant;
    public HeroModel enemyPrefab
    {
        get
        {
            return Tavern.Instance.GetInfoHero(heroName.ToString());
        }
    }

    public int level = 1;
}
public enum HeroName
{
    Peasant = 1101,
    Robin = 1102,
    Militia = 1201,
    Raccoon = 2101,
    Pegasus = 2401,
    Spirit = 3101,
    DeathKnight = 3501,
    Imp = 4101,
    Demoniac = 4102,
    TempleGuard = 5301
}