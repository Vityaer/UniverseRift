using City.Panels.SelectHeroes;
using Hero;
using Models.City.TrainCamp;
using Models.Heroes;
using System;
using TMPro;
using UniRx;
using UnityEngine;
using UnityEngine.UI;

namespace UIController.Cards
{
    public class Card : MonoBehaviour, IDisposable
    {
        public GameHero Hero;
        public bool Selected = false;

        [SerializeField] private Image _imageUI;
        [SerializeField] private TextMeshProUGUI _levelUI;
        [SerializeField] private Image _panelSelect;
        [SerializeField] private VocationView _vocationUI;
        [SerializeField] private RaceView _raceUI;
        [SerializeField] private Button Button;

        private RatingHero _ratingController;
        private ReactiveCommand<Card> _onClick = new ReactiveCommand<Card>();
        private CompositeDisposable _disposables = new CompositeDisposable();
        private IDisposable _heroSubscribe;
        public IObservable<Card> OnClick => _onClick;

        private void Awake()
        {
            Button.OnClickAsObservable().Subscribe(_ => ClickOnCard()).AddTo(_disposables);
        }

        public void SetData(RequirementHeroModel requirementHero)
        {
            gameObject.SetActive(true);
            _levelUI.text = string.Empty;
            _ratingController.ShowRating(requirementHero.rating);
            // vocationUI.SetData(requirementHero.);
            // raceUI.SetData(requirementHero.);
            SetImage(requirementHero.GetData);
        }

        public void SetData(GameHero hero)
        {   
            if (Selected)
                Unselect();

            Hero = hero;
            UpdateUI();
            _heroSubscribe = hero.OnChangeData.Subscribe(_ => UpdateUI());
        }

        private void UpdateUI()
        {
            _imageUI.sprite = Hero.Avatar;
            _levelUI.text = $"{Hero.HeroData.Level}";
            //_ratingController.ShowRating(Hero.General.RatingHero);

        }

        private void SetImage(HeroModel data)
        {
            //_imageUI.sprite = data.General.ImageHero;
        }

        //API
        public void ClickOnCard()
        {
            _onClick.Execute(this);
        }

        public void Select()
        {
            Selected = true;
            _panelSelect.enabled = true;
        }

        public void Unselect()
        {
            Selected = false;
            _panelSelect.enabled = false;
        }

        public void Clear()
        {
            _imageUI.sprite = null;
            _levelUI.text = string.Empty;
            //_ratingController.Hide();
            gameObject.SetActive(false);
            _heroSubscribe?.Dispose();
        }

        public void Dispose()
        {
            _heroSubscribe.Dispose();
            _disposables.Dispose();
        }
    }
}