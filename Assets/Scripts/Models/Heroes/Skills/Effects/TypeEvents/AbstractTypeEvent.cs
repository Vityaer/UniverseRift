using Fight.HeroControllers.Generals;
using Newtonsoft.Json;
using UniRx;

namespace Models.Heroes.Skills.Effects.TypeEvents
{
    [System.Serializable]
    public abstract class AbstractTypeEvent
    {
        private ReactiveCommand _onExecute = new();

        [JsonIgnore] public ReactiveCommand OnExecute => _onExecute;

        public abstract void Subscribe(HeroController master, CompositeDisposable disposables);

        protected void Execute()
        {
            _onExecute.Execute();
        }
    }
}
