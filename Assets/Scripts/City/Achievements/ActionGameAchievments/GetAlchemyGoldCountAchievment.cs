using Models.Common.BigDigits;
using UIController.ControllerPanels.AlchemyPanels;
using UniRx;
using VContainer;

namespace City.Achievements.ActionGameAchievments
{
    public class GetAlchemyGoldCountAchievment : GameAchievment
    {
        [Inject] private readonly AlchemyPanelController m_alchemyPanelController;

        protected override void Subscribe()
        {
            m_alchemyPanelController.OnGetAchemyGold.Subscribe(_ => AddProgress(new BigDigit(1)))
                .AddTo(Disposables);
        }
    }
}