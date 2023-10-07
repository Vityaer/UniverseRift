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

        private string _currentRace;
        private ReactiveCommand<TravelRaceCampaignButton> _onSelect = new();

        public IObservable<TravelRaceCampaignButton> OnSelect => _onSelect;

        private void Awake()
        {
            MainButton.onClick.AddListener(() => _onSelect.Execute(this));
        }

        public void SetData(string newRace)
        {
            _imageRace.sprite = SystemSprites.Instance.GetSprite(newRace);
            _currentRace = newRace;
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