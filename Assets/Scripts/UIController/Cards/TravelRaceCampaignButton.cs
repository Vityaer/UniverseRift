using Models.City.TravelCircle;
using System;
using TMPro;
using UIController.ItemVisual;
using UniRx;
using UnityEngine;
using UnityEngine.UI;

namespace UIController.Cards
{
    public class TravelRaceCampaignButton : MonoBehaviour
    {
        [SerializeField] private Image _imageRace;
        [SerializeField] private Button MainButton;
        [SerializeField] private GameObject _outLine;
        [SerializeField] private TMP_Text _progressText;
        [SerializeField][Space(10)] private string _currentRace;

        private ReactiveCommand<TravelRaceCampaignButton> _onSelect = new();

        public string RaceId => _currentRace;
        public IObservable<TravelRaceCampaignButton> OnSelect => _onSelect;

        private void Awake()
        {
            MainButton.onClick.AddListener(() => _onSelect.Execute(this));
        }

        public void SetData(TravelRaceModel travelModel, int missionIndexCompleted)
        {
            var currentMission = missionIndexCompleted >= 0 ? missionIndexCompleted + 1 : 0;
            _progressText.text = $"{currentMission} / {travelModel.Missions.Count}";
        }

        public void Select()
        {
            _outLine.SetActive(true);
        }

        public void Diselect()
        {
            _outLine.SetActive(false);
        }

    }
}