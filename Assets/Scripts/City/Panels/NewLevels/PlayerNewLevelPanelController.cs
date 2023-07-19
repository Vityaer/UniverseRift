using Models.Common;
using TMPro;
using UIController.Rewards;
using UiExtensions.Scroll.Interfaces;
using VContainer;

namespace City.Panels.NewLevels
{
    public class PlayerNewLevelPanelController : UiPanelController<PlayerNewLevelPanelView>
    {
        [Inject] private readonly CommonGameData _�ommonGameData;

        public void SetData(RewardModel reward)
        {
            var newLevel = _�ommonGameData.Player.PlayerInfoData.Level;
            View.NewLevelLabel.text = $"{newLevel}";
        }
    }
}