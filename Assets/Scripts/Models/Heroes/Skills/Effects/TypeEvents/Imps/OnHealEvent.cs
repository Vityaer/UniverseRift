using Fight.HeroControllers.Generals;
using UniRx;

namespace Models.Heroes.Skills.Effects.TypeEvents.Imps
{
    public class OnHealEvent : AbstractTypeEvent
    {
        public override void Subscribe(HeroController master, CompositeDisposable disposables)
        {
            master.OnHeal.Subscribe(_ => Execute()).AddTo(disposables);
        }
    }
}
