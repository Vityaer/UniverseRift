using Fight.Common.HeroControllers.Generals;
using Models.Heroes.Skills.Effects.TypeEvents;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UniRx;

namespace Models.Heroes.Skills.Effects.TypeEvents.Imps
{
    public class OnDeathHeroEvent : AbstractTypeEvent
    {
        public override void Subscribe(HeroController master, CompositeDisposable disposables)
        {
            master.OnDeath.Subscribe(_ => Execute()).AddTo(disposables);
        }
    }
}
