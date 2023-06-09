using System.Collections.Generic;

namespace Fight.Comparers
{
    public class WarriorArmorComparer : IComparer<Warrior>
    {
        public int Compare(Warrior w1, Warrior w2)
        {
            if (w1.heroController.hero.characts.GeneralArmor < w2.heroController.hero.characts.GeneralArmor)
                return 1;
            else if (w1.heroController.hero.characts.GeneralArmor > w2.heroController.hero.characts.GeneralArmor)
                return -1;
            else
                return 0;
        }
    }
}
