using Hero;
using System;
using Common.Db.CommonDictionaries;
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
        [Inject] private CommonDictionaries m_commonDictionaries;

        public GameHero Hero;
        public bool Selected = false;

        [SerializeField] private Image _imageUI;
        [SerializeField] private TextMeshProUGUI _levelUI;
        [SerializeField] private Image _panelSelect;
        [SerializeField] private CardDetailImageView _vocationUI;
        [SerializeField] private RatingHero _ratingController;
        [SerializeField] private CardDetailImageView _vocation;
        [SerializeField] private CardDetailImageView _race;

        private ReactiveCommand<Card> m_onClick = new ReactiveCommand<Card>();
        private CompositeDisposable m_disposables = new CompositeDisposable();
        private IDisposable m_heroSubscribe;

        public IObservable<Card> OnClick => m_onClick;

        protected override void Awake()
        {
            Button.OnClickAsObservable().Subscribe(_ => ClickOnCard()).AddTo(m_disposables);
        }

        public override void SetData(GameHero data, ScrollRect scrollRect, CommonDictionaries commonDictionaries)
        {
            base.SetData(data, scrollRect, commonDictionaries);
            SetData(data);
        }

        public void SetData(GameHero hero)
        {
            if (Selected)
                Unselect();

            Hero = hero;
            UpdateUI();
            m_heroSubscribe = hero.OnChangeData.Subscribe(_ => UpdateUI());
        }

        private void UpdateUI()
        {
            _imageUI.sprite = Hero.Avatar;
            _vocation.SetData(m_commonDictionaries.Vocations[Hero.Model.General.Vocation].SpritePath);
            _race.SetData(m_commonDictionaries.Races[Hero.Model.General.Race].SpritePath);
            _levelUI.text = $"{Hero.HeroData.Level}";
            _ratingController.ShowRating(Hero.HeroData.Rating);
        }

        //API
        private void ClickOnCard()
        {
            m_onClick.Execute(this);
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
            m_heroSubscribe?.Dispose();
        }


        public void SetRace(string race)
        {
            _race.SetData(m_commonDictionaries.Races[race].SpritePath);
        }

        public override void Dispose()
        {
            m_heroSubscribe?.Dispose();
            m_disposables?.Dispose();
        }
    }
}