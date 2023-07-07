using Common.Resourses;
using Fight;
using Models.Fights.Campaign;
using System;
using TMPro;
using UIController;
using UniRx;
using UnityEngine;
using UnityEngine.UI;

namespace Campaign
{
    public class MissionController : MonoBehaviour
    {
        [Header("UI")]
        public TextMeshProUGUI textNameMission;
        public TextMeshProUGUI textAutoRewardGold;
        public TextMeshProUGUI textAutoRewardExperience;
        public TextMeshProUGUI textAutoRewardStone;
        public Image backgoundMission;
        public GameObject infoFotter;
        public GameObject blockPanel;
        public GameObject imageAutoFight;
        public GameObject btnGoFight;
        public TextMeshProUGUI textBtnGoFight;
        public RewardUIController rewardController;
        [Header("Contollers")]
        public StatusMission Status;
        public CampaignMissionModel mission;
        public int numMission;
        private ReactiveCommand<MissionController> _onClickMission = new ReactiveCommand<MissionController>();

        public IObservable<MissionController> OnClickMission => _onClickMission;

        public void SetMission(CampaignMissionModel mission, int numMission)
        {
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
                textAutoRewardGold.text = $"{autoReward.BaseResource[ResourceType.Gold].Amount} /{Constants.Game.TACT_TIME}sec.";
                textAutoRewardStone.text = $"{autoReward.BaseResource[ResourceType.ContinuumStone].Amount} /{Constants.Game.TACT_TIME}sec.";
                textAutoRewardExperience.text = $"{autoReward.BaseResource[ResourceType.Exp].Amount} /{Constants.Game.TACT_TIME}sec.";
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
                    //rewardController.ShowReward(mission.WinReward);
                    rewardController.OpenReward();
                    textBtnGoFight.text = "Вызвать";
                    break;
                case StatusMission.Complete:
                case StatusMission.InAutoFight:
                    //rewardController.ShowAutoReward(mission.AutoFightReward);
                    rewardController.OpenReward();
                    textBtnGoFight.text = "Авто";
                    break;
            }
            btnGoFight.SetActive(Status == StatusMission.Open || Status == StatusMission.Complete);
            blockPanel.SetActive(false);
        }
    }
}