using Hero;

namespace Models.Fights.Campaign
{
    public abstract class AbstractHeroRestriction
    {
        public abstract bool CheckHero(GameHero hero);
    }
}