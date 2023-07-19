using Models.Heroes;
using System;
using UIController.Cards;
using UniRx;
using UnityEngine;

namespace City.Buildings.TravelCircle
{
    public class TravelSelect : MonoBehaviour
    {
        [SerializeField] private GameObject _selectBorder;
        public string Race;
        public RaceView RaceUI;
        private static TravelSelect selectedRace = null;
        private ReactiveCommand<string> _onSelect = new ReactiveCommand<string>();

        public IObservable<string> OnSelect => _onSelect;

        void Start()
        {
            RaceUI.SetData(Race);

        }

        public void Open()
        {
            _onSelect.Execute(Race);
        }

        public void Select()
        {
            if (selectedRace != null) selectedRace.Diselect();
            selectedRace = this;
            _selectBorder.SetActive(true);
        }

        public void Diselect()
        {
            _selectBorder.SetActive(false);
        }
    }
}