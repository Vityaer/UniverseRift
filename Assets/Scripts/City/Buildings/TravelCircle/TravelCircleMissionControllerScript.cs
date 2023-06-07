using Assets.Scripts.City.Buildings.TravelCircle;
using City.Buildings.BaseObjectsUI;
using Models.Fights.Campaign;
using TMPro;
using UIController;
using UIController.Panels;
using UnityEngine;
using UnityEngine.UI;

namespace City.Buildings.TravelCircle
{
    public class TravelCircleMissionControllerScript : BaseMissionController
    {
        [Header("UI")]
        public RewardUIController rewardController;
        public TextMeshProUGUI textNumMission, textOnButtonSelect;
        public GameObject buttonSelect, imageCloseMission;
        public Image backgoundMission;
        [SerializeField] private Button mainButton;

        private MissionWithSmashReward mission;
        private StatusMission status = StatusMission.NotOpen;
        private int numMission = 0;

        private void Start()
        {
            mainButton.onClick.AddListener(OpenMission);
        }

        public void SetData(MissionWithSmashReward mission, int numMission)
        {
            gameObject.SetActive(true);
            status = StatusMission.NotOpen;
            this.mission = mission;
            this.numMission = numMission;
            UpdateUI();
        }

        private void UpdateUI()
        {
            textNumMission.text = numMission.ToString();
            switch (status)
            {
                case StatusMission.Open:
                    rewardController.ShowReward(mission.WinReward);
                    textOnButtonSelect.text = "Вызвать";
                    buttonSelect.SetActive(true);
                    imageCloseMission.SetActive(false);
                    break;
                case StatusMission.InAutoFight:
                    rewardController.ShowReward(mission.SmashReward);
                    textOnButtonSelect.text = "Рейд";
                    buttonSelect.SetActive(true);
                    imageCloseMission.SetActive(false);
                    break;
                case StatusMission.NotOpen:
                    rewardController.ShowReward(mission.WinReward);
                    buttonSelect.SetActive(false);
                    imageCloseMission.SetActive(true);
                    break;
            }
        }

        public void OpenForFight()
        {
            status = StatusMission.Open;
            UpdateUI();
        }

        public void Hide()
        {
            status = StatusMission.Complete;
            gameObject.SetActive(false);
        }

        public void SetCanSmash()
        {
            status = StatusMission.InAutoFight;
            UpdateUI();
        }

        public void OpenMission()
        {
            switch (status)
            {
                case StatusMission.Open:
                    TravelCircleScript.Instance.OpenMission(mission);
                    break;
                case StatusMission.InAutoFight:
                    break;
                case StatusMission.NotOpen:
                    MessageController.Instance.AddMessage("Миссия ещё не открыта");
                    break;
            }
        }

        protected override ParentScroll GetScrollParent()
        {
            return TravelCircleScript.Instance.scrollRectController;
        }
    }
}