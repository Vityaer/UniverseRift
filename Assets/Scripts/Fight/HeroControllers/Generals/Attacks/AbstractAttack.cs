using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Fight.HeroControllers.Generals.Attacks
{
    public abstract class AbstractAttack : IAttackable
    {
        public abstract void Attack();

        public void MakeDamage(){}
    }
}
