using Common.Resourses;
using LocalizationSystems;
using Models.Common.BigDigits;
using Models.Fights.Campaign;
using System;
using Common.Db.CommonDictionaries;
using TMPro;
using UIController;
using UniRx;
using UnityEngine;
using UnityEngine.Localization.Components;
using UnityEngine.Localization.SmartFormat.PersistentVariables;
using UnityEngine.UI;
using VContainer;

namespace Campaign
{
    public class MissionController : MonoBehaviour
    {
        [Inject] private ILocalizationSystem _localizationSystem;

        [Header("UI")]
        public TextMeshProUGUI textNameMission;
        public LocalizeStringEvent AutoRewardGoldText;
        public LocalizeStringEvent AutoRewardExperienceText;
        public LocalizeStringEvent AutoRewardStoneText;
        public Image backgoundMission;
        public GameObject infoFotter;
        public GameObject blockPanel;
        public GameObject imageAutoFight;
        public GameObject btnGoFight;
        public LocalizeStringEvent ButtonActionLocalizeText;
        public RewardUIController rewardController;

        [Header("Contollers")]
        public StatusMission Status;
        public CampaignMissionModel mission;
        public int numMission;

        private ReactiveCommand<MissionController> _onClickMission = new();
        private CommonDictionaries _commonDictionaries;

        public IObservable<MissionController> OnClickMission => _onClickMission;

        public void SetMission(CampaignMissionModel mission, int numMission, CommonDictionaries commonDictionaries)
        {
            _commonDictionaries = commonDictionaries;
            this.mission = mission;
            this.numMission = numMission;
            //backgoundMission.sprite = LocationController.Instance.GetBackgroundForMission(this.mission.Location);
            textNameMission.text = numMission.ToString();
            Status = StatusMission.NotOpen;
            UpdateUI();
        }

        public void ClickOnMission()
        {
            _onClickMission.Execute(this);
        }

        public void UpdateAutoRewardUI()
        {
            if (Status == StatusMission.Complete || Status == StatusMission.InAutoFight)
            {
                infoFotter.SetActive(true);
                var autoReward = mission.AutoFightReward;
                SetAutoResourceData(AutoRewardGoldText, autoReward.BaseResource[ResourceType.Gold].Amount);
                SetAutoResourceData(AutoRewardStoneText, autoReward.BaseResource[ResourceType.ContinuumStone].Amount);
                SetAutoResourceData(AutoRewardExperienceText, autoReward.BaseResource[ResourceType.Exp].Amount);
            }
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

        public void StartAutoFight()
        {
            Status = StatusMission.InAutoFight;
            imageAutoFight.SetActive(true);
            btnGoFight.SetActive(false);
        }

        public void StopAutoFight()
        {
            Status = StatusMission.Complete;
            imageAutoFight.SetActive(false);
            btnGoFight.SetActive(true);
        }

        public void MissionWin()
        {
            Status = StatusMission.InAutoFight;
            UpdateAutoRewardUI();
            UpdateUI();
            StartAutoFight();
        }

        public void CompletedMission()
        {
            Status = StatusMission.Complete;
            imageAutoFight.SetActive(false);
            UpdateUI();
            UpdateAutoRewardUI();
        }

        public void OpenMission()
        {
            Status = StatusMission.Open;
            blockPanel.SetActive(false);
            UpdateUI();
        }

        private void UpdateUI()
        {
            switch (Status)
            {
                case StatusMission.NotOpen:
                    blockPanel.SetActive(true);
                    infoFotter.SetActive(false);
                    btnGoFight.SetActive(false);
                    rewardController.CloseReward();
                    break;
                case StatusMission.Open:
                    rewardController.ShowReward(mission.WinReward, _commonDictionaries);
                    rewardController.OpenReward();
                    ButtonActionLocalizeText.StringReference = _localizationSystem
                        .GetLocalizedContainer("MissionStartFightButtonLabel");
                    break;
                case StatusMission.Complete:
                case StatusMission.InAutoFight:
                    rewardController.ShowAutoReward(mission.AutoFightReward, _commonDictionaries);
                    rewardController.OpenReward();
                    ButtonActionLocalizeText.StringReference = _localizationSystem
                        .GetLocalizedContainer("MissionAutoFightButtonLabel");
                    break;
            }
            btnGoFight.SetActive(Status == StatusMission.Open || Status == StatusMission.Complete);
            blockPanel.SetActive(false);
        }
    }
}