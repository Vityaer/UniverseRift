using City.Panels.SubjectPanels.Common;
using ClientServices;
using Common.Resourses;
using Common.Rewards;
using LocalizationSystems;
using Models.Common.BigDigits;
using Models.Data.Rewards;
using Services.TimeLocalizeServices;
using System;
using Common.Db.CommonDictionaries;
using UiExtensions.Panels;
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
        [Inject] private readonly CommonDictionaries m_commonDictionaries;
        [Inject] private readonly IUiMessagesPublisherService m_uiMessagesPublisher;
        [Inject] private readonly SubjectDetailController m_subjectDetailController;
        [Inject] private readonly ILocalizationSystem m_localizationSystem;
        [Inject] private readonly TimeLocalizeService m_timeLocalizeService;
        [Inject] private readonly ClientRewardService m_clientRewardService;

        private readonly TimeSpan m_maxTime = new TimeSpan(12, 0, 0);
        private GameReward m_calculatedReward;

        public readonly ReactiveCommand OnClose = new();

        public override void Start()
        {
            View.RewardUIController.SetDetailsController(m_subjectDetailController);
            View.AccumulationSlider.Init(m_localizationSystem, m_timeLocalizeService);
            base.Start();
        }

        public void Open(AutoRewardData autoReward, GameReward calculatedReward, DateTime previousDateTime)
        {
            m_calculatedReward = calculatedReward;
            SetAutoResourceData(View.AutoRewardGoldText, autoReward.BaseResource[ResourceType.Gold].Amount);
            SetAutoResourceData(View.AutoRewardStoneText, autoReward.BaseResource[ResourceType.ContinuumStone].Amount);
            SetAutoResourceData(View.AutoRewardExperienceText, autoReward.BaseResource[ResourceType.Exp].Amount);

            View.RewardUIController.ShowReward(calculatedReward, m_commonDictionaries, lengthReward: true);
            View.AccumulationSlider.SetData(previousDateTime, m_maxTime);
            m_uiMessagesPublisher.OpenWindowPublisher.OpenWindow<AutoFightRewardPanelController>(openType: OpenType.Exclusive);
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
            m_clientRewardService.GetReward(m_calculatedReward);
        }

    }
}