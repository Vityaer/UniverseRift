using Fight.HeroControllers.Generals;
using Sirenix.OdinInspector;
using UniRx;
using UnityEngine;

namespace Models.Heroes.Skills.Effects.TypeEvents.Imps
{
    public class OnHealthLessPercentEvent : AbstractTypeEvent
    {
        [SerializeField] private float _targetHealthLessPercent;
        [SerializeField] private bool _limited = true;
        [ShowIf("_limited")][SerializeField] private int _countCanExecute = 1;

        public override void Subscribe(HeroController master, CompositeDisposable disposables)
        {
            if (_limited)
            {
                master.OnHealthChangePercent
                    .Where(value => value.Item2 < _targetHealthLessPercent)
                    .Take(_countCanExecute)
                    .Subscribe(_ => Execute())
                    .AddTo(disposables);
            }
            else
            {
                master.OnHealthChangePercent
                    .Where(value => value.Item2 < _targetHealthLessPercent)
                    .Subscribe(_ => Execute())
                    .AddTo(disposables);
            }
        }
    }
}
