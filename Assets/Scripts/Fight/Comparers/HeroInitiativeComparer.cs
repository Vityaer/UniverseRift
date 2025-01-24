using Fight.HeroControllers.Generals;
using System.Collections.Generic;

namespace Fight.Comparers
{
    public class HeroInitiativeComparer : IComparer<HeroController>
    {
        public int Compare(HeroController hero1, HeroController hero2)
        {
            if (hero1.Hero.Model.Characteristics.Initiative < hero2.Hero.Model.Characteristics.Initiative)
                return 1;
            else if (hero1.Hero.Model.Characteristics.Initiative > hero2.Hero.Model.Characteristics.Initiative)
                return -1;
            else
                return 0;
        }
    }
}
