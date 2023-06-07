using System.Collections.Generic;

namespace Fight.Comparers
{
    public class WarriorInitiativeComparer : IComparer<Warrior>
    {
        public int Compare(Warrior w1, Warrior w2)
        {
            if (w1.heroController.hero.characts.Initiative < w2.heroController.hero.characts.Initiative)
                return 1;
            else if (w1.heroController.hero.characts.Initiative > w2.heroController.hero.characts.Initiative)
                return -1;
            else
                return 0;
        }
    }
}
