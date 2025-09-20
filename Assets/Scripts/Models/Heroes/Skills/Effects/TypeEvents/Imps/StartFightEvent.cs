using Fight.Common.HeroControllers.Generals;
using UniRx;

namespace Models.Heroes.Skills.Effects.TypeEvents.Imps
{
    public class StartFightEvent : AbstractTypeEvent
    {
        public override void Subscribe(HeroController master, CompositeDisposable disposables)
        {
            master.OnStartFight.Subscribe(_ => Execute()).AddTo(disposables);
        }
    }
}
