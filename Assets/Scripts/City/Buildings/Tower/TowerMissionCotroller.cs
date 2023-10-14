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
        [Header("UI")]
        public TextMeshProUGUI textNumMission;
        public Image backgoundMission, enemyImage;
        public RewardUIController rewardController;
        public GameObject blockPanel;

        private int numMission = 0;
        public bool canOpenMission = false;

        private ReactiveCommand<TowerMissionCotroller> _onClick = new ReactiveCommand<TowerMissionCotroller>();

        public IObservable<TowerMissionCotroller> OnClick => _onClick;

        protected override void Start()
        {
            Button.OnClickAsObservable().Subscribe(_ => OnMissionClick()).AddTo(Disposable);
        }

        public override void SetData(MissionModel data, ScrollRect scrollRect)
        {
            Scroll = scrollRect;
            Data = data;
        }

        public void SetData(MissionModel mission, ScrollRect scrollRect, int numMission, bool canOpenMission = false)
        {
            SetData(mission, scrollRect);
            Data = mission;
            this.numMission = numMission;
            this.canOpenMission = canOpenMission;
            UpdateUI();
        }

        private void UpdateUI()
        {
            textNumMission.text = numMission.ToString();
            if (Data?.WinReward != null)
                rewardController.ShowReward(Data.WinReward);

            blockPanel.SetActive(canOpenMission == false);
        }

        private void OnMissionClick()
        {
            if (canOpenMission)
            {
                _onClick.Execute(this);
            }
        }
    }
}