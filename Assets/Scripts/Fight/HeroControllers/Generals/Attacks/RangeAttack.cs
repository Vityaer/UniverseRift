using System.Collections;
using System.Collections.Generic;
using UniRx;
using UnityEngine;

namespace Fight.HeroControllers.Generals.Attacks
{
    public class RangeAttack : AbstractAttack
    {
        public Arrow Prefab;

        private CompositeDisposable _disposables;
        private int _hitTargetCount;
        private int _hitCurrentCount;
        private List<HeroController> _targets = new();
        private bool _finishAttack;

        public override void Attack(List<HeroController> targets)
        {
            Refresh(targets.Count);

            _targets = targets;
            foreach (var target in targets)
                CreateArrow(target);
        }

        public override void Attack(HeroController target)
        {
            Refresh();
            _targets.Add(target);
        }

        private void Refresh(int countTarget = 1)
        {
            _disposables = new();
            _hitCurrentCount = 0;
            _hitTargetCount = countTarget;
            _targets.Clear();
        }

        private void CreateArrows()
        {
            foreach (var target in _targets)
                CreateArrow(target);
        }

        private void CreateArrow(HeroController target)
        {
            var arrow = Instantiate(Prefab, transform.position, Quaternion.identity);
            arrow.SetTarget(target);
            arrow.OnReachTarget.Subscribe(OnReachTarget).AddTo(_disposables);
        }

        private void OnReachTarget(HeroController target)
        {
            OnRangeDamage.Execute(target);
            _hitCurrentCount += 1;

            if (_hitCurrentCount == _hitTargetCount)
            {
                _disposables.Dispose();
                _disposables = null;
                _finishAttack = true;
            }
        }

        public override IEnumerator Attacking(HeroController target, int bonus)
        {
            _finishAttack = false;
            Attack(target);
            while(!_finishAttack)
                yield return null;
            
        }
    }
}
