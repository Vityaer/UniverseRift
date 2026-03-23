using System.Collections.Generic;
using Cysharp.Threading.Tasks;
using Fight.Common;
using UniRx;
using UnityEngine;

namespace Fight.HeroControllers.Generals.Attacks
{
    public class RangeAttack : AbstractAttack
    {
        public Arrow Prefab;

        private CompositeDisposable m_disposables;
        private int m_hitTargetCount;
        private int m_hitCurrentCount;
        private List<HeroController> m_targets = new();
        private bool m_finishAttack;

        protected override void Attack(List<HeroController> targets)
        {
            Refresh(targets.Count);

            m_targets = targets;
            foreach (HeroController target in targets)
                CreateArrow(target);
        }

        public override void Attack(HeroController target)
        {
            Refresh();
            m_targets.Add(target);
        }

        private void Refresh(int countTarget = 1)
        {
            m_disposables = new();
            m_hitCurrentCount = 0;
            m_hitTargetCount = countTarget;
            m_targets.Clear();
        }

        private void CreateArrows()
        {
            foreach (HeroController target in m_targets)
                CreateArrow(target);
        }

        private void CreateArrow(HeroController target)
        {
            var arrow = Instantiate(Prefab, transform.position, Quaternion.identity);
            arrow.SetTarget(target);
            arrow.OnReachTarget.Subscribe(OnReachTarget).AddTo(m_disposables);
        }

        private void OnReachTarget(Fight.HeroControllers.Generals.HeroController target)
        {
            OnRangeDamage.Execute(target);
            m_hitCurrentCount += 1;

            if (m_hitCurrentCount != m_hitTargetCount)
            {
                return;
            }
            
            m_disposables.Dispose();
            m_disposables = null;
            m_finishAttack = true;
        }

        public override async UniTask Attacking(HeroController target, int bonus)
        {
            Hero.StatusState.ChangeStamina(bonus);
            Attack(target);

            if (!Hero.IsFastFight)
            {
                m_finishAttack = false;
                await UniTask.WaitUntil(() => m_finishAttack);
            }
            else
            {
                foreach (HeroController hero in m_targets)
                {
                    OnRangeDamage.Execute(hero);
                }
                
                m_finishAttack = true;
            }
        }
    }
}