using City.Panels.Messages;
using Models.Fights.Campaign;
using System;
using TMPro;
using UIController;
using UiExtensions.Misc;
using UniRx;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

namespace City.Buildings.Tower
{
    public class TowerMissionCotroller : ScrollableUiView<MissionModel>
    {
        private MissionModel mission;
        [Header("UI")]
        public TextMeshProUGUI textNumMission;
        public Image backgoundMission, enemyImage;
        public RewardUIController rewardController;
        public GameObject blockPanel;

        private int numMission = 0;
        public bool canOpenMission = false;

        private ReactiveCommand<TowerMissionCotroller> _onClick = new ReactiveCommand<TowerMissionCotroller>();

        public IObservable<TowerMissionCotroller> OnClick => _onClick;

        public override void SetData(MissionModel data, ScrollRect scrollRect)
        {
            Scroll = scrollRect;
            mission = data;
        }

        public void SetData(MissionModel mission, int numMission, bool canOpenMission = false)
        {
            this.mission = mission;
            this.numMission = numMission;
            this.canOpenMission = canOpenMission;
            UpdateUI();
        }

        private void UpdateUI()
        {
            textNumMission.text = numMission.ToString();
            //if (mission != null)
            //    rewardController.ShowReward(mission.WinReward);
            blockPanel.SetActive(canOpenMission == false);
        }

        public void OpenMission()
        {
            if (canOpenMission)
            {
                _onClick.Execute(this);
            }
            else
            {
                //MessageController.Instance.AddMessage("Миссия ещё не открыта");
            }
        }


    }
}