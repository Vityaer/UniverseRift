using System.Collections.Generic;
using Cysharp.Threading.Tasks;

namespace Fight.Common.HeroControllers.Generals.Attacks
{
    public interface IAttackable
    {
        void MakeDamage();
        UniTask Attacking(Fight.HeroControllers.Generals.HeroController target, int bonus);
    }
}
