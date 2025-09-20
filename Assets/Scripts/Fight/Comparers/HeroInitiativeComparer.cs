using System.Collections.Generic;
using Fight.Common.HeroControllers.Generals;

namespace Fight.Common.Comparers
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
