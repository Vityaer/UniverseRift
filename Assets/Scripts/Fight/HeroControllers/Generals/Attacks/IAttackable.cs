using System.Collections.Generic;
using Cysharp.Threading.Tasks;

namespace Fight.HeroControllers.Generals.Attacks
{
    public interface IAttackable
    {
        void MakeDamage();
        UniTask Attacking(HeroController target, int bonus);
    }
}
