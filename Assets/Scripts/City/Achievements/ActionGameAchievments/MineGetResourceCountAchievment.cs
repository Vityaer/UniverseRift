using City.Buildings.Mines;
using Models.Common.BigDigits;
using UniRx;
using VContainer;

namespace City.Achievements.ActionGameAchievments
{
    public class MineGetResourceCountAchievment : GameAchievment
    {
        [Inject] private readonly MinesPageController _minesPageController;

        protected override void Subscribe()
        {
            _minesPageController.OnGetResource.Subscribe(_ => AddProgress(new BigDigit(1))).AddTo(Disposables);
        }
    }
}
