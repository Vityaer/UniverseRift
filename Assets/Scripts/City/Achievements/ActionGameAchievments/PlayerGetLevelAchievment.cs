using City.Buildings.PlayerPanels;
using UniRx;
using VContainer;

namespace City.Achievements.ActionGameAchievments
{
    public class PlayerGetLevelAchievment  : GameAchievment
    {
        [Inject] private readonly PlayerPanelController _playerPanelController;

        protected override void Subscribe()
        {
            _playerPanelController.OnLevelUp.Subscribe(AddProgress).AddTo(Disposables);
        }
    }
}