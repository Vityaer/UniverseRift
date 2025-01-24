using System.Collections;
using System.Collections.Generic;

namespace Fight.HeroControllers.Generals.Attacks
{
    public interface IAttackable
    {
        void Attack(List<HeroController> targets);
        void Attack(HeroController target);

        void MakeDamage();
        IEnumerator Attacking(HeroController target, int bonus);
    }
}
