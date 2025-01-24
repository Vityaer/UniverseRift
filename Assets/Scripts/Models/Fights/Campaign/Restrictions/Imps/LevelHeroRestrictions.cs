using Hero;

namespace Models.Fights.Campaign.Restrictions.Imps
{
    public class LevelHeroRestrictions : AbstractHeroRestriction
    {
        public int Level;

        public override bool CheckHero(GameHero hero)
        {
            return hero.HeroData.Level >= Level;
        }
    }
}
