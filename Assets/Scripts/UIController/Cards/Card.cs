using Db.CommonDictionaries;
using Hero;
using System;
using TMPro;
using UiExtensions.Misc;
using UniRx;
using UnityEngine;
using UnityEngine.UI;
using VContainer;

namespace UIController.Cards
{
    public class Card : ScrollableUiView<GameHero>
    {
        [Inject] private CommonDictionaries _commonDictionaries;

        public GameHero Hero;
        public bool Selected = false;

        [SerializeField] private Image _imageUI;
        [SerializeField] private TextMeshProUGUI _levelUI;
        [SerializeField] private Image _panelSelect;
        [SerializeField] private CardDetailImageView _vocationUI;
        [SerializeField] private RatingHero _ratingController;
        [SerializeField] private CardDetailImageView _vocation;
        [SerializeField] private CardDetailImageView _race;

        private ReactiveCommand<Card> _onClick = new ReactiveCommand<Card>();
        private CompositeDisposable _disposables = new CompositeDisposable();
        private IDisposable _heroSubscribe;

        public IObservable<Card> OnClick => _onClick;

        protected override void Awake()
        {
            Button.OnClickAsObservable().Subscribe(_ => ClickOnCard()).AddTo(_disposables);
        }

        public override void SetData(GameHero data, ScrollRect scrollRect)
        {
            base.SetData(data, scrollRect);
            SetData(data);
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
            _vocation.SetData(_commonDictionaries.Vocations[Hero.Model.General.Vocation].SpritePath);
            _race.SetData(_commonDictionaries.Races[Hero.Model.General.Race].SpritePath);
            _levelUI.text = $"{Hero.HeroData.Level}";
            _ratingController.ShowRating(Hero.HeroData.Rating);

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
            gameObject.SetActive(false);
            _heroSubscribe?.Dispose();
        }


        public void SetRace(string race)
        {
            _race.SetData(_commonDictionaries.Races[race].SpritePath);
        }

        public override void Dispose()
        {
            _heroSubscribe?.Dispose();
            _disposables?.Dispose();
        }
    }
}