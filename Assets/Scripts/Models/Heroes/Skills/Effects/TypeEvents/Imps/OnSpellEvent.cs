using Fight.Common.HeroControllers.Generals;
using UniRx;

namespace Models.Heroes.Skills.Effects.TypeEvents.Imps
{
    public class OnSpellEvent : AbstractTypeEvent
    {
        public override void Subscribe(HeroController master, CompositeDisposable disposables)
        {
            master.OnSpell.Subscribe(_ => Execute()).AddTo(disposables);
        }
    }
}
