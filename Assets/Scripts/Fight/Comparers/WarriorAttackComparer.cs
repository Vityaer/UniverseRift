using System.Collections.Generic;

namespace Fight.Comparers
{
    public class WarriorAttackComparer : IComparer<Warrior>
    {
        public int Compare(Warrior w1, Warrior w2)
        {
            if (w1.heroController.hero.characts.Damage < w2.heroController.hero.characts.Damage)
                return 1;
            else if (w1.heroController.hero.characts.Damage > w2.heroController.hero.characts.Damage)
                return -1;
            else
                return 0;
        }
    }
}
