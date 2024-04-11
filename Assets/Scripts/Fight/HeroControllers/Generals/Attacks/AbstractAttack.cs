using System.Collections.Generic;
using UniRx;
using UnityEngine;

namespace Fight.HeroControllers.Generals.Attacks
{
    public abstract class AbstractAttack : MonoBehaviour, IAttackable
    {
        public ReactiveCommand OnFinishAttack = new();
        public ReactiveCommand OnMakeDamage = new();
        public ReactiveCommand<HeroController> OnRangeDamage = new();
        public abstract void Attack(List<HeroController> targets);
        public abstract void Attack(HeroController target);

        public void MakeDamage()
        {
            OnMakeDamage.Execute();
        }
    }
}
