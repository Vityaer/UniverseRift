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
using Utils.DataCheckUtils;
using VContainer;

namespace City.Buildings.Tower
{
    public class TowerMissionController : ScrollableUiView<MissionModel>
    {
        [Inject] private CommonDictionaries m_commonDictionaries;
        [Inject] private HeroInstancesController m_heroInstancesController;

        [Header("UI")]
        public TextMeshProUGUI textNumMission;
        public Image backgoundMission;
        public Image enemyImage;
        public RewardUIController rewardController;
        public GameObject blockPanel;

        private int m_numMission = 0;
        public bool canOpenMission = false;

        private ReactiveCommand<TowerMissionController> m_onClick = new();

        public IObservable<TowerMissionController> OnClick => m_onClick;

        [Inject]
        private void Construct(SubjectDetailController subjectDetailController)
        {
            rewardController.SetDetailsController(subjectDetailController);
        }

        protected override void Start()
        {
            Button.OnClickAsObservable().Subscribe(_ => OnMissionClick()).AddTo(Disposable);
        }

        public override void SetData(MissionModel data, ScrollRect scrollRect, CommonDictionaries commonDictionaries)
        {
            Scroll = scrollRect;
            Data = data;
            CommonDataDictionaries = commonDictionaries.ThrowIfArgumentNull();
            m_commonDictionaries = commonDictionaries;
        }

        public void SetData(MissionModel mission, ScrollRect scrollRect, int numMission, bool canOpenMission = false)
        {
            SetData(mission, scrollRect, m_commonDictionaries);
            Data = mission;
            this.m_numMission = numMission;
            this.canOpenMission = canOpenMission;

            if (mission.Units.Count > 0)
            {
                var model = mission.Units[0];
                var prefab = m_heroInstancesController.GetHeroPrefab(model.HeroId);
                var stage = (model.Rating / 5);
                enemyImage.sprite = prefab.Stages[stage].Avatar;
            }

            UpdateUI();
        }

        private void UpdateUI()
        {
            textNumMission.text = m_numMission.ToString();
            if (Data?.WinReward != null)
                rewardController.ShowReward(Data.WinReward, m_commonDictionaries);



            blockPanel.SetActive(canOpenMission == false);
        }

        private void OnMissionClick()
        {
            if (canOpenMission)
            {
                m_onClick.Execute(this);
            }
        }
    }
}