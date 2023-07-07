using Common.Resourses;
using System;
using UIController.Rewards;
using UiExtensions.Scroll.Interfaces;
using VContainer;
using VContainerUi.Model;
using VContainerUi.Services;
using VContainerUi.Messages;
using UniRx;
using DG.Tweening;
using UnityEngine;
using Common.Rewards;
using Models.Data.Rewards;
using UIController;

namespace City.Panels.AutoFights
{
    public class AutoFightRewardPanelController : UiPanelController<AutoFightRewardPanelView>
    {
        [Inject] protected readonly IUiMessagesPublisherService UiMessagesPublisher;

        private TimeSpan maxTime = new TimeSpan(12, 0, 0);
        public ReactiveCommand OnClose = new ReactiveCommand();

        public void Open(AutoRewardData autoReward, GameReward calculatedReward, DateTime previousDateTime)
        {
            View.textAutoRewardGold.text = $"{autoReward.BaseResource[ResourceType.Gold].Amount} /{Constants.Game.TACT_TIME}sec.";
            View.textAutoRewardStone.text = $"{autoReward.BaseResource[ResourceType.ContinuumStone].Amount} /{Constants.Game.TACT_TIME}sec.";
            View.textAutoRewardExperience.text = $"{autoReward.BaseResource[ResourceType.Exp].Amount} /{Constants.Game.TACT_TIME}sec.";

            View.RewardUIController.ShowAllReward(calculatedReward);
            View.sliderAccumulation.SetData(previousDateTime, maxTime);
            UiMessagesPublisher.OpenWindowPublisher.OpenWindow<AutoFightRewardPanelController>(openType: OpenType.Additive);
        }

        protected override void Close()
        {
            OnClose.Execute();
            base.Close();
        }

    }
}