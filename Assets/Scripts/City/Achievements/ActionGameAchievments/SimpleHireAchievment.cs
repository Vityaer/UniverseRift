using City.Buildings.Tavern;
using Cysharp.Threading.Tasks;
using UniRx;
using VContainer;

namespace City.Achievements.ActionGameAchievments
{
    public class SimpleHireAchievment : GameAchievment
    {
        [Inject] private readonly TavernController _tavernController;

        protected override void Subscribe()
        {
            _tavernController.ObserverSimpleHire.Subscribe(AddProgress).AddTo(Disposables);
        }
    }
}
