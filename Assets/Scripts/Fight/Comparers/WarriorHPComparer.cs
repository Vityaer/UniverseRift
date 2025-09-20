using System.Collections.Generic;

namespace Fight.Common.Comparers
{
    public class WarriorHPComparer : IComparer<Warrior>
    {
        public int Compare(Warrior w1, Warrior w2)
        {
            if (w1.heroController.Hero.Model.Characteristics.HP < w2.heroController.Hero.Model.Characteristics.HP)
                return 1;
            else if (w1.heroController.Hero.Model.Characteristics.HP > w2.heroController.Hero.Model.Characteristics.HP)
                return -1;
            else
                return 0;
        }
    }
}
