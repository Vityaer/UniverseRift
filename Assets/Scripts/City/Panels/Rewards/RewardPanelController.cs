using City.Panels.ScreenBlockers;
using Common.Db.CommonDictionaries;
using Common.Rewards;
using UiExtensions.Panels;
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
        [Inject] private readonly CommonDictionaries m_commonDictionaries;
        [Inject] private readonly ScreenBlockerController m_screenBlockerController;

        private GameObject m_currentLabel;

        private bool m_isOpen;
        public readonly ReactiveCommand OnClose = new ReactiveCommand();

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
            View.RewardUIController.ShowReward(reward, m_commonDictionaries, fast);

            m_currentLabel?.SetActive(false);

            switch (rewardType)
            {
                case RewardType.Simple:
                    m_currentLabel = View.SimpleRewardLabel;
                    break;
                case RewardType.Win:
                    m_currentLabel = View.WinLabel;
                    break;
                case RewardType.Defeat:
                    m_currentLabel = View.DefeatLabel;
                    break;
            }

            if (m_currentLabel != null)
                m_currentLabel.SetActive(true);
            UiMessagesPublisher.OpenWindowPublisher.OpenWindow<RewardPanelController>(openType: OpenType.Additive);
        }

        protected override void Close()
        {
            m_isOpen = false;
            m_currentLabel.SetActive(false);
            OnClose.Execute();
            base.Close();
        }
    }
}