using ClientServices;
using Common.Rewards;
using Models.Common;
using UIController.Rewards;
using UiExtensions.Scroll.Interfaces;
using VContainer;
using VContainerUi.Model;
using VContainerUi.Messages;
using Db.CommonDictionaries;

namespace City.Panels.NewLevels
{
    public class PlayerNewLevelPanelController : UiPanelController<PlayerNewLevelPanelView>
    {
        [Inject] private readonly CommonGameData _ñommonGameData;
        [Inject] private readonly ClientRewardService _clientRewardService;
        [Inject] private readonly CommonDictionaries _commonDictionaries;
        private GameReward _gameReward;

        public void SetData(RewardModel reward)
        {
            View.RewardUIController.ShowReward(reward, _commonDictionaries);
            _gameReward = new GameReward(reward, _commonDictionaries);
            var newLevel = _ñommonGameData.PlayerInfoData.Level;
            View.NewLevelLabel.text = $"{newLevel}";
            MessagesPublisher.OpenWindowPublisher.OpenWindow<PlayerNewLevelPanelController>(openType: OpenType.Exclusive);
        }

        protected override void Close()
        {
            _clientRewardService.GetReward(_gameReward);
            base.Close();
        }
    }
}