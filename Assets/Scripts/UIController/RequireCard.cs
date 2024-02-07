using City.Panels.SelectHeroes;
using Hero;
using Models.City.TrainCamp;
using System;
using System.Collections.Generic;
using TMPro;
using UIController.Cards;
using UniRx;
using UnityEngine;
using UnityEngine.UI;
using Utils;

namespace UIController
{
    public class RequireCard : MonoBehaviour, IDisposable
    {
        [SerializeField] private Card card;
        [SerializeField] private TextMeshProUGUI textCountRequirement;
        [SerializeField] private Image _icon;

        [Header("Panel select heroes")]
        public HeroCardsContainerController listCard;
        public Button Button;

        private int _requireSelectCount = 0;
        private RequirementHeroModel _requirementHero;
        private List<GameHero> _selectedHeroes = new();

        private CompositeDisposable _disposables;
        private IDisposable _buttonDisposed;

        private ReactiveCommand<RequireCard> _onClick = new();

        public IObservable<RequireCard> OnClick => _onClick;
        public RequirementHeroModel RequirementHero => _requirementHero;
        public List<GameHero> SelectedHeroes => _selectedHeroes;

        private void Start()
        {
            _buttonDisposed = Button?.OnClickAsObservable().Subscribe(_ => OpenListCard());
        }

        public void SetData(RequirementHeroModel requirementHero)
        {
            _icon.sprite = SpriteUtils.LoadSprite(requirementHero.IconPath);
            ClearData();
            _requirementHero = requirementHero;
            _requireSelectCount = requirementHero.Count;
            card.SetData(requirementHero);
            UpdateUI();
        }

        public void SetData(RequirementHeroModel requirementHero, List<GameHero> selectedHeroes)
        {
            _icon.sprite = SpriteUtils.LoadSprite(requirementHero.IconPath);
            _selectedHeroes = new();
            foreach (var hero in selectedHeroes)
                _selectedHeroes.Add(hero);

            _requirementHero = requirementHero;
            _requireSelectCount = requirementHero.Count;
            card.SetData(requirementHero);
            UpdateUI();
        }

        private void AddHero(GameHero hero)
        {
            Debug.Log($"add hero, current: {_selectedHeroes.Count}");
            if (_selectedHeroes.Count < _requireSelectCount)
            {
                _selectedHeroes.Add(hero);
                UpdateUI();
            }
            else
            {
                listCard.UnselectCard(hero);
            }
        }

        private void RemoveHero(GameHero hero)
        {
            Debug.Log($"remove hero, current: {_selectedHeroes.Count}");
            if (_selectedHeroes.Count > 0)
            {
                _selectedHeroes.Remove(hero);
                UpdateUI();
            }
        }

        private void UpdateUI()
        {
            textCountRequirement.text = $"{_selectedHeroes.Count}/{_requireSelectCount}";
        }

        public bool CheckHeroes()
        {
            bool result = false;
            return result;
        }

        public void OpenListCard()
        {
            _disposables = new();
            _onClick.Execute(this);
            listCard.OnSelect.Subscribe(AddHero).AddTo(_disposables);
            listCard.OnDiselect.Subscribe(RemoveHero).AddTo(_disposables);
        }

        public void OnCloseListCard()
        {
            if (_disposables != null && !_disposables.IsDisposed)
            {
                _disposables.Dispose();
            }
        }

        public void ClearData()
        {
            _selectedHeroes.Clear();
        }

        public void Hide()
        {
            gameObject.SetActive(false);
        }



        public void Dispose()
        {
            _buttonDisposed?.Dispose();
            if (_disposables != null && !_disposables.IsDisposed)
            {
                _disposables.Dispose();
            }
        }
    }
}