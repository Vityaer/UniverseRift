using Fight.HeroControllers.Generals;
using UniRx;

namespace Models.Heroes.Skills.Effects.TypeEvents.Imps
{
    public class OnTakeDamage : AbstractTypeEvent
    {
        public override void Subscribe(HeroController master, CompositeDisposable disposables)
        {
            master.OnTakeDamage.Subscribe(_ => Execute()).AddTo(disposables);
        }
    }
}
