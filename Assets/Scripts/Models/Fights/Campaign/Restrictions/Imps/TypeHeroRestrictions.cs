using Hero;
using Sirenix.OdinInspector;
using System.Linq;

namespace Models.Fights.Campaign.Restrictions.Imps
{
    public class TypeHeroRestrictions : AbstractHeroRestriction
    {
        public string Race;

        public override bool CheckHero(GameHero hero)
        {
            return hero.Model.General.Race.Equals(Race);
        }
    }
}
