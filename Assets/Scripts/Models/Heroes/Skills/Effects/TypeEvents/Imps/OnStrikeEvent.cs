using Fight.Common.HeroControllers.Generals;
using UniRx;
using HeroController = Fight.HeroControllers.Generals.HeroController;

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
