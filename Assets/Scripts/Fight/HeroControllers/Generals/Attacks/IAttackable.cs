using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Fight.HeroControllers.Generals.Attacks
{
    public interface IAttackable
    {
        void Attack(List<HeroController> targets);
        void Attack(HeroController target);

        void MakeDamage();
    }
}
