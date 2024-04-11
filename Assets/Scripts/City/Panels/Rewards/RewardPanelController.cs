using City.Panels.AutoFights;
using Common.Rewards;
using UiExtensions.Scroll.Interfaces;
using UniRx;
using UnityEngine;
using VContainer;
using VContainerUi.Messages;
using VContainerUi.Model;
using VContainerUi.Services;

namespace City.Panels.Rewards
{
    public class RewardPanelController : UiPanelController<RewardPanelView>
    {
        [Inject] protected readonly IUiMessagesPublisherService UiMessagesPublisher;

        private GameObject _currentLabel;

        public ReactiveCommand OnClose = new ReactiveCommand();

        public void Open(GameReward reward, RewardType rewardType)
        {
            View.RewardUIController.ShowReward(reward);
            switch (rewardType)
            {
                case RewardType.Simple:
                    _currentLabel = View.SimpleRewardLabel;
                    break;
                case RewardType.Win:
                    _currentLabel = View.WinLabel;
                    break;
                case RewardType.Defeat:
                    _currentLabel = View.DefeatLabel;
                    break;

            }
            _currentLabel.SetActive(true);
            UiMessagesPublisher.OpenWindowPublisher.OpenWindow<RewardPanelController>(openType: OpenType.Exclusive);
        }

        protected override void Close()
        {
            _currentLabel.SetActive(false);
            OnClose.Execute();
            base.Close();
        }
    }
}
