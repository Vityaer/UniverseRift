using ClientServices;
using Common.Resourses;
using Common.Rewards;
using Models.Data.Rewards;
using System;
using System.Diagnostics;
using UIController.Rewards;
using UiExtensions.Scroll.Interfaces;
using UniRx;
using VContainer;
using VContainerUi.Messages;
using VContainerUi.Model;
using VContainerUi.Services;

namespace City.Panels.AutoFights
{
    public class AutoFightRewardPanelController : UiPanelController<AutoFightRewardPanelView>
    {
        [Inject] protected readonly IUiMessagesPublisherService UiMessagesPublisher;
        [Inject] private readonly ClientRewardService _clientRewardService;

        private TimeSpan maxTime = new TimeSpan(12, 0, 0);
        public ReactiveCommand OnClose = new ReactiveCommand();
        private GameReward _calculatedReward;
        public void Open(AutoRewardData autoReward, GameReward calculatedReward, DateTime previousDateTime)
        {
            _calculatedReward = calculatedReward;
            View.textAutoRewardGold.text = $"{autoReward.BaseResource[ResourceType.Gold].Amount} /{Constants.Game.TACT_TIME}sec.";
            View.textAutoRewardStone.text = $"{autoReward.BaseResource[ResourceType.ContinuumStone].Amount} /{Constants.Game.TACT_TIME}sec.";
            View.textAutoRewardExperience.text = $"{autoReward.BaseResource[ResourceType.Exp].Amount} /{Constants.Game.TACT_TIME}sec.";

            View.RewardUIController.ShowReward(calculatedReward);
            View.sliderAccumulation.SetData(previousDateTime, maxTime);
            UiMessagesPublisher.OpenWindowPublisher.OpenWindow<AutoFightRewardPanelController>(openType: OpenType.Exclusive);
        }

        protected override void Close()
        {
            OnClose.Execute();
            base.Close();
            _clientRewardService.GetReward(_calculatedReward);
        }

    }
}