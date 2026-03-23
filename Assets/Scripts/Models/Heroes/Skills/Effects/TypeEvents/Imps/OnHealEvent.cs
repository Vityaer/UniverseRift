using Fight.Common.HeroControllers.Generals;
using UniRx;
using HeroController = Fight.HeroControllers.Generals.HeroController;

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
