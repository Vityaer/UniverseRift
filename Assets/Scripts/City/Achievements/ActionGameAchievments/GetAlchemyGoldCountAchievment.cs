using City.Panels.Arenas.SimpleArenas;
using Models.Common.BigDigits;
using UIController.ControllerPanels.AlchemyPanels;
using UniRx;
using VContainer;

namespace City.Achievements.ActionGameAchievments
{
    public class GetAlchemyGoldCountAchievment : GameAchievment
    {
        [Inject] private readonly AlchemyPanelController _alchemyPanelController;

        protected override void Subscribe()
        {
            _alchemyPanelController.OnGetAchemyGold.Subscribe(_ => AddProgress(new BigDigit(1))).AddTo(Disposables);
        }
    }
}
