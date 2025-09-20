using Fight.Common.HeroControllers.Generals;
using UniRx;

namespace Models.Heroes.Skills.Effects.TypeEvents.Imps
{
    public class OnStrikeEvent : AbstractTypeEvent
    {
        public override void Subscribe(HeroController master, CompositeDisposable disposables)
        {
            master.OnStrike.Subscribe(_ => Execute()).AddTo(disposables);
        }
    }
}
