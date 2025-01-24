using City.Panels.SubjectPanels.Common;
using ClientServices;
using Common.Resourses;
using Common.Rewards;
using LocalizationSystems;
using Models.Common.BigDigits;
using Models.Data.Rewards;
using Services.TimeLocalizeServices;
using System;
using UiExtensions.Scroll.Interfaces;
using UniRx;
using UnityEngine.Localization.Components;
using UnityEngine.Localization.SmartFormat.PersistentVariables;
using VContainer;
using VContainerUi.Messages;
using VContainerUi.Model;
using VContainerUi.Services;

namespace City.Panels.AutoFights
{
    public class AutoFightRewardPanelController : UiPanelController<AutoFightRewardPanelView>
    {
        [Inject] private readonly IUiMessagesPublisherService _uiMessagesPublisher;
        [Inject] private readonly SubjectDetailController _subjectDetailController;
        [Inject] private readonly ILocalizationSystem _localizationSystem;
        [Inject] private readonly TimeLocalizeService _timeLocalizeService;
        [Inject] private readonly ClientRewardService _clientRewardService;

        private TimeSpan maxTime = new TimeSpan(12, 0, 0);
        private GameReward _calculatedReward;

        public ReactiveCommand OnClose = new();

        public override void Start()
        {
            View.RewardUIController.SetDetailsController(_subjectDetailController);
            View.AccumulationSlider.Init(_localizationSystem, _timeLocalizeService);
            base.Start();
        }

        public void Open(AutoRewardData autoReward, GameReward calculatedReward, DateTime previousDateTime)
        {
            _calculatedReward = calculatedReward;
            SetAutoResourceData(View.AutoRewardGoldText, autoReward.BaseResource[ResourceType.Gold].Amount);
            SetAutoResourceData(View.AutoRewardStoneText, autoReward.BaseResource[ResourceType.ContinuumStone].Amount);
            SetAutoResourceData(View.AutoRewardExperienceText, autoReward.BaseResource[ResourceType.Exp].Amount);

            View.RewardUIController.ShowReward(calculatedReward, lengthReward: true);
            View.AccumulationSlider.SetData(previousDateTime, maxTime);
            _uiMessagesPublisher.OpenWindowPublisher.OpenWindow<AutoFightRewardPanelController>(openType: OpenType.Exclusive);
        }

        private void SetAutoResourceData(LocalizeStringEvent localizeStringEvent, BigDigit amount)
        {
            if (localizeStringEvent.StringReference.TryGetValue("Resource", out var variable))
            {
                var stringVariable = variable as StringVariable;
                stringVariable.Value = $"{amount}";
            }
            if (localizeStringEvent.StringReference.TryGetValue("Time", out var timeVariable))
            {
                var stringVariable = timeVariable as StringVariable;
                stringVariable.Value = $"{Constants.Game.TACT_TIME}";
            }
        }

        protected override void Close()
        {
            OnClose.Execute();
            base.Close();
            _clientRewardService.GetReward(_calculatedReward);
        }

    }
}