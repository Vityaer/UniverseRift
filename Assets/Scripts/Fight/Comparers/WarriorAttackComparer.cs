using System.Collections.Generic;

namespace Fight.Comparers
{
    public class WarriorAttackComparer : IComparer<Warrior>
    {
        public int Compare(Warrior w1, Warrior w2)
        {
            if (w1.heroController.Hero.Model.Characteristics.Damage < w2.heroController.Hero.Model.Characteristics.Damage)
                return 1;
            else if (w1.heroController.Hero.Model.Characteristics.Damage > w2.heroController.Hero.Model.Characteristics.Damage)
                return -1;
            else
                return 0;
        }
    }
}
