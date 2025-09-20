using City.Panels.SubjectPanels.Common;
using City.TrainCamp.HeroInstances;
using Models.Fights.Campaign;
using System;
using Common.Db.CommonDictionaries;
using TMPro;
using UIController;
using UiExtensions.Misc;
using UniRx;
using UnityEngine;
using UnityEngine.UI;
using VContainer;

namespace City.Buildings.Tower
{
    public class TowerMissionCotroller : ScrollableUiView<MissionModel>
    {
        [Inject] private CommonDictionaries _commonDictionaries;
        [Inject] private HeroInstancesController _heroInstancesController;

        [Header("UI")]
        public TextMeshProUGUI textNumMission;
        public Image backgoundMission;
        public Image enemyImage;
        public RewardUIController rewardController;
        public GameObject blockPanel;

        private int numMission = 0;
        public bool canOpenMission = false;

        private ReactiveCommand<TowerMissionCotroller> _onClick = new ReactiveCommand<TowerMissionCotroller>();

        public IObservable<TowerMissionCotroller> OnClick => _onClick;

        [Inject]
        private void Construct(SubjectDetailController subjectDetailController)
        {
            rewardController.SetDetailsController(subjectDetailController);
        }

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

            if (mission.Units.Count > 0)
            {
                var model = mission.Units[0];
                var prefab = _heroInstancesController.GetHero(model.HeroId);
                var stage = (model.Rating / 5);
                enemyImage.sprite = prefab.Stages[stage].Avatar;
            }

            UpdateUI();
        }

        private void UpdateUI()
        {
            textNumMission.text = numMission.ToString();
            if (Data?.WinReward != null)
                rewardController.ShowReward(Data.WinReward, _commonDictionaries);



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