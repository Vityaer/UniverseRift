using City.Panels.ScreenBlockers;
using Common.Rewards;
using Cysharp.Threading.Tasks;
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
        [Inject] private readonly ScreenBlockerController _screenBlockerController;

        private GameObject _currentLabel;

        private bool m_isOpen;
        public ReactiveCommand OnClose = new ReactiveCommand();

        public override void Start()
        {
            View.SimpleRewardLabel.SetActive(false);
            View.WinLabel.SetActive(false);
            View.DefeatLabel.SetActive(false);
            base.Start();
        }

        public void Open(GameReward reward, RewardType rewardType, bool fast = true)
        {
            if (m_isOpen)
                return;
            
            m_isOpen = true;
            View.RewardUIController.ShowReward(reward, fast);

            _currentLabel?.SetActive(false);

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
            UiMessagesPublisher.OpenWindowPublisher.OpenWindow<RewardPanelController>(openType: OpenType.Additive);
        }

        protected override void Close()
        {
            m_isOpen = false;
            _currentLabel.SetActive(false);
            OnClose.Execute();
            base.Close();
        }
    }
}