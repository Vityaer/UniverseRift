using City.Buildings.Tavern;
using Models.Heroes;

namespace Campaign
{
    [System.Serializable]
    public class Unit
    {
        public string Name;
        public int Level = 1;

        public HeroModel Prefab => Tavern.Instance.GetInfoHero(Name);

    }
}