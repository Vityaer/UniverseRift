using Fight.HeroControllers.Generals;
using System.Collections.Generic;

namespace Fight.Comparers
{
    public class HeroInitiativeComparer : IComparer<HeroController>
    {
        public int Compare(HeroController hero1, HeroController hero2)
        {
            if (hero1.hero.characts.Initiative < hero2.hero.characts.Initiative)
                return 1;
            else if (hero1.hero.characts.Initiative > hero2.hero.characts.Initiative)
                return -1;
            else
                return 0;
        }
    }
}
