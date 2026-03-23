using City.Panels.SubjectPanels.Common;
using LocalizationSystems;
using Models.Fights.Campaign;
using System;
using Common.Db.CommonDictionaries;
using TMPro;
using UIController;
using UiExtensions.Misc;
using UniRx;
using UnityEngine;
using UnityEngine.Localization.Components;
using UnityEngine.UI;
using Utils.DataCheckUtils;
using VContainer;

namespace City.Buildings.TravelCircle
{
    public class TravelCircleMissionController : ScrollableUiView<MissionWithSmashReward>
    {
        [Inject] private CommonDictionaries m_commonDictionaries;
        [Inject] private ILocalizationSystem m_localizationSystem;

        [Header("UI")]
        public RewardUIController rewardController;
        public TMP_Text NumMissionText;
        public LocalizeStringEvent OnButtonSelectLacalizeString;

        public GameObject buttonSelect, imageCloseMission;
        public Image backgoundMission;
        [SerializeField] private Button mainButton;

        private MissionWithSmashReward m_mission;
        private StatusMission m_status = StatusMission.NotOpen;
        private int m_numMission = 0;

        private ReactiveCommand<TravelCircleMissionController> m_onSelect = new();

        public new IObservable<TravelCircleMissionController> OnSelect => m_onSelect;
        public StatusMission Status => m_status;
        public int Index => m_numMission;

        [Inject]
        private void Construct(SubjectDetailController subjectDetailController)
        {
            rewardController.SetDetailsController(subjectDetailController);
        }

        private new void Start()
        {
            mainButton.onClick.AddListener(OpenMission);
            base.Start();
        }

        public override void SetData(MissionWithSmashReward data, ScrollRect scrollRect, CommonDictionaries commonDictionaries)
        {
            Scroll = scrollRect.ThrowIfArgumentNull();
            Data = data;
            CommonDataDictionaries = commonDictionaries.ThrowIfArgumentNull();
        }

        public void SetData(MissionWithSmashReward mission, ScrollRect scrollRect, int numMission, bool canOpenMission = false)
        {
            SetData(mission, scrollRect, m_commonDictionaries);
            gameObject.SetActive(true);
            m_status = StatusMission.NotOpen;
            this.m_mission = mission;
            this.m_numMission = numMission;
            UpdateUI();
        }

        private void UpdateUI()
        {
            NumMissionText.text = $"{m_numMission}";
            switch (m_status)
            {
                case StatusMission.Open:
                    rewardController.ShowReward(m_mission.WinReward, m_commonDictionaries);
                    OnButtonSelectLacalizeString.StringReference = m_localizationSystem
                        .GetLocalizedContainer("MissionStartFightButtonLabel");
                    buttonSelect.SetActive(true);
                    imageCloseMission.SetActive(false);
                    break;
                case StatusMission.InAutoFight:
                    rewardController.ShowReward(m_mission.SmashReward, m_commonDictionaries);
                    OnButtonSelectLacalizeString.StringReference = m_localizationSystem
                        .GetLocalizedContainer("MissionRaidFightButtonLabel");
                    buttonSelect.SetActive(true);
                    imageCloseMission.SetActive(false);
                    break;
                case StatusMission.NotOpen:
                    rewardController.ShowReward(m_mission.WinReward, m_commonDictionaries);
                    buttonSelect.SetActive(false);
                    imageCloseMission.SetActive(true);
                    break;
            }
        }

        public void OpenForFight()
        {
            m_status = StatusMission.Open;
            UpdateUI();
        }

        public void Hide()
        {
            m_status = StatusMission.Complete;
            gameObject.SetActive(false);
        }

        public void SetCanSmash()
        {
            Debug.Log("SetCanSmash");
            m_status = StatusMission.InAutoFight;
            UpdateUI();
        }

        public void OpenMission()
        {
            m_onSelect.Execute(this);
        }
    }
}