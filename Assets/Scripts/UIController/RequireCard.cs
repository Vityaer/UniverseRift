using City.Panels.SelectHeroes;
using City.TrainCamp;
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
    public class RequireCard : MonoBehaviour
    {
        [SerializeField] private Card card;
        [SerializeField] private TextMeshProUGUI textCountRequirement;
        [SerializeField] private Image _icon;
        [SerializeField] private TMP_Text _level;
        [SerializeField] private RatingHero _ratingController;

        [Header("Panel select heroes")]
        public HeroCardsContainerController listCard;
        public Button Button;

        private int _requireSelectCount = 0;
        private RequirementHeroModel _requirementHero;
        private List<GameHero> _selectedHeroes = new();

        private CompositeDisposable _disposables;
        private IDisposable _buttonDisposed;

        private ReactiveCommand<RequireCard> _onClick = new();
        private GameHero _currentHero;

        public IObservable<RequireCard> OnClick => _onClick;
        public RequirementHeroModel RequirementHero => _requirementHero;
        public List<GameHero> SelectedHeroes => _selectedHeroes;
        public Card Card => card;

        private void Start()
        {
            _buttonDisposed = Button?.OnClickAsObservable().Subscribe(_ => OpenListCard());
        }

        public void SetData(GameHero currentHero, RequirementHeroModel requirementHero)
        {
            _currentHero = currentHero;
            switch (requirementHero.RequireRace)
            {
                case EvolutionRequireType.EqualHero:
                    var stage = (requirementHero.Rating / 5);
                    _icon.sprite = currentHero.Prefab.Stages[stage].Avatar;
                    break;
                default:
                    _icon.sprite = SpriteUtils.LoadSprite(requirementHero.IconPath);
                    break;
            }
            ClearData();
            _requirementHero = requirementHero;
            _requireSelectCount = requirementHero.Count;
            _level.text = string.Empty;
            _ratingController.ShowRating(requirementHero.Rating);
            card.SetRace(currentHero.Model.General.Race);
            UpdateUI();
        }

        public void SetProgress(RequirementHeroModel requirementHero, List<GameHero> selectedHeroes)
        {
            _icon.sprite = SpriteUtils.LoadSprite(requirementHero.IconPath);
            _selectedHeroes = new();
            foreach (var hero in selectedHeroes)
                _selectedHeroes.Add(hero);

            _requirementHero = requirementHero;
            _requireSelectCount = requirementHero.Count;
            UpdateUI();
        }

        private void AddHero(GameHero hero)
        {
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
            if (_requireSelectCount == 0)
            {
                return true;
            }

            return _selectedHeroes.Count == _requireSelectCount;
        }

        private void OpenListCard()
        {
            OnOpenList();
            _onClick.Execute(this);
        }

        public void OnOpenList()
        {
            _disposables = new();
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

        private void OnDestroy()
        {
            _buttonDisposed?.Dispose();
            if (_disposables != null && !_disposables.IsDisposed)
            {
                _disposables.Dispose();
            }
        }
    }
}