using City.Achievements;
using City.Buildings.CityButtons.EventAgent;
using City.Panels.SubjectPanels.Common;
using Db.CommonDictionaries;
using TMPro;
using UIController;
using UiExtensions.Misc;
using UnityEngine;
using UnityEngine.UI;
using VContainer;

namespace City.Panels.BatllepasPanels
{
    public class BattlepasRewardView : ScrollableUiView<GameBattlepasReward>
    {
        [SerializeField] private GameObject _donePanel;
        [SerializeField] private RewardUIController _rewardController;
        [SerializeField] private TMP_Text NumberText;

        [Inject] private CommonDictionaries _commonDictionaries;

        [Inject]
        private void Construct(SubjectDetailController subjectDetailController)
        {
            _rewardController.SetDetailsController(subjectDetailController);
        }

        public override void SetData(GameBattlepasReward data, ScrollRect scrollRect)
        {
            Data = data;
            Scroll = scrollRect;
            _rewardController.ShowReward(data.RewardModel, _commonDictionaries);
            NumberText.text = $"{transform.GetSiblingIndex()}";
            UpdateUI();
        }

        private void UpdateUI()
        {
        }

        public override void SetStatus(ScrollableViewStatus status)
        {
            switch (status)
            {
                case ScrollableViewStatus.Completed:
                case ScrollableViewStatus.Close:
                    _donePanel.SetActive(true);
                    break;
                case ScrollableViewStatus.Open:
                    _donePanel.SetActive(false);
                    break;
            }
        }

    }
}
