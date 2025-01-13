using Fight.Common.Strikes;
using System;
using System.Collections;
using System.Collections.Generic;
using UniRx;
using UnityEngine;

namespace Fight.HeroControllers.Generals.Attacks
{
    public abstract class AbstractAttack : MonoBehaviour, IAttackable
    {
        protected HeroController Hero;
        protected IDisposable Disposable;

        public ReactiveCommand OnFinishAttack = new();
        public ReactiveCommand OnMakeDamage = new();
        public ReactiveCommand<HeroController> OnRangeDamage = new();


        public void Init(HeroController heroController)
        {
            Hero = heroController;
        }

        public virtual void Attack(List<HeroController> targets)
        {
            TryDispose();
            foreach (var target in targets)
            {
                target.ApplyDamage(
                    new Strike
                    (
                        Hero.Hero.Model.Characteristics.Damage,
                        Hero.Hero.Model.Characteristics.Main.Attack,
                        typeStrike: TypeStrike.Physical
                    )
                );
            }
        }

        public virtual void Attack(HeroController target)
        {
            TryDispose();

            target.ApplyDamage(
                new Strike
                (
                    Hero.Hero.Model.Characteristics.Damage,
                    Hero.Hero.Model.Characteristics.Main.Attack,
                    typeStrike: TypeStrike.Physical
                )
            );
        }

        public abstract IEnumerator Attacking(HeroController target, int bonus);

        public void MakeDamage()
        {
            OnMakeDamage.Execute();
        }

        protected void TryDispose()
        {
            Disposable?.Dispose();
            Disposable = null;
        }

        private void OnDestroy()
        {
            TryDispose();
        }
    }
}
