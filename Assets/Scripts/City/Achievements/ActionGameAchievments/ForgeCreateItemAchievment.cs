using City.Buildings.Forge;
using VContainer;
using UniRx;

namespace City.Achievements.ActionGameAchievments
{
    public class ForgeCreateItemAchievment : GameAchievment
    {
        [Inject] private readonly ForgeController _forgeController;

        protected override void Subscribe()
        {
            _forgeController.OnCraft
                .Subscribe(AddProgress)
                .AddTo(Disposables);

        }
    }
}