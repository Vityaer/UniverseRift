using Fight.HeroControllers.Generals;

namespace Fight.Statistics
{
    public class HeroStatistic
    {
        private HeroController hero;
        public HeroController Hero { get => hero; }

        private float damageDone;
        public float DamageDone { get => damageDone; }

        private float healDone;
        public float HealDone { get => healDone; }

        public HeroStatistic(HeroController hero)
        {
            this.hero = hero;
        }
    }
}
