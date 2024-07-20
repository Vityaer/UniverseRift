using City.Panels.SubjectPanels.Common;
using Db.CommonDictionaries;
using Models.Fights.Campaign;
using System;
using TMPro;
using UIController;
using UiExtensions.Misc;
using UniRx;
using UnityEngine;
using UnityEngine.UI;
using VContainer;

namespace City.Buildings.TravelCircle
{
    public class TravelCircleMissionController : ScrollableUiView<MissionWithSmashReward>
    {
        [Inject] private CommonDictionaries _commonDictionaries;

        [Header("UI")]
        public RewardUIController rewardController;
        public TextMeshProUGUI textNumMission, textOnButtonSelect;
        public GameObject buttonSelect, imageCloseMission;
        public Image backgoundMission;
        [SerializeField] private Button mainButton;

        private MissionWithSmashReward _mission;
        private StatusMission _status = StatusMission.NotOpen;
        private int _numMission = 0;

        private new ReactiveCommand<TravelCircleMissionController> _onSelect = new();

        public new IObservable<TravelCircleMissionController> OnSelect => _onSelect;
        public StatusMission Status => _status;
        public int Index => _numMission;

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

        public override void SetData(MissionWithSmashReward data, ScrollRect scrollRect)
        {
            Scroll = scrollRect;
            Data = data;
        }

        public void SetData(MissionWithSmashReward mission, ScrollRect scrollRect, int numMission, bool canOpenMission = false)
        {
            SetData(mission, scrollRect);
            gameObject.SetActive(true);
            _status = StatusMission.NotOpen;
            this._mission = mission;
            this._numMission = numMission;
            UpdateUI();
        }

        private void UpdateUI()
        {
            textNumMission.text = $"{_numMission}";
            switch (_status)
            {
                case StatusMission.Open:
                    rewardController.ShowReward(_mission.WinReward, _commonDictionaries);
                    textOnButtonSelect.text = "Вызвать";
                    buttonSelect.SetActive(true);
                    imageCloseMission.SetActive(false);
                    break;
                case StatusMission.InAutoFight:
                    rewardController.ShowReward(_mission.SmashReward, _commonDictionaries);
                    textOnButtonSelect.text = "Рейд";
                    buttonSelect.SetActive(true);
                    imageCloseMission.SetActive(false);
                    break;
                case StatusMission.NotOpen:
                    rewardController.ShowReward(_mission.WinReward, _commonDictionaries);
                    buttonSelect.SetActive(false);
                    imageCloseMission.SetActive(true);
                    break;
            }
        }

        public void OpenForFight()
        {
            _status = StatusMission.Open;
            UpdateUI();
        }

        public void Hide()
        {
            _status = StatusMission.Complete;
            gameObject.SetActive(false);
        }

        public void SetCanSmash()
        {
            Debug.Log("SetCanSmash");
            _status = StatusMission.InAutoFight;
            UpdateUI();
        }

        public void OpenMission()
        {
            _onSelect.Execute(this);
        }
    }
}