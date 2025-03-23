using Cysharp.Threading.Tasks;
using Hero;
using Models.City.TrainCamp;
using System;
using System.Collections.Generic;
using UIController;
using UniRx;
using UnityEngine;

namespace City.TrainCamp
{
    public class ListRequirementHeroesUI : MonoBehaviour
    {
        public HeroEvolutionPanelController MainController;

        [SerializeField] private List<RequireCard> _requireCards = new();
        private List<RequirementHeroModel> _requirementHeroes = new();
        private CompositeDisposable _disposables = new();
        private ReactiveCommand<RequireCard> _onSelectRequireCard = new();

        public List<RequirementHeroModel> SelectedHeroes => _requirementHeroes;


        public IObservable<RequireCard> OnSelectRequireCard => _onSelectRequireCard;
        public List<RequireCard> RequireCards => _requireCards;

        private void Start()
        {
            foreach (var card in _requireCards)
            {
                card.OnClick.Subscribe(SelectRequireCard).AddTo(_disposables);
            }
        }

        private void SelectRequireCard(RequireCard card)
        {
            _onSelectRequireCard.Execute(card);
        }

        public void SetData(GameHero currentHero, List<RequirementHeroModel> requirementHeroes)
        {
            _requirementHeroes = requirementHeroes;
            for (int i = 0; i < _requireCards.Count; i++)
            {
                if (i < requirementHeroes.Count)
                {
                    _requireCards[i].SetData(currentHero, requirementHeroes[i]);
                }
                else
                {
                    _requireCards[i].Hide();
                }
            }
        }

        public void HeroSelectDiselect()
        {
            MainController.CheckHeroes();
        }

        public bool IsAllDone()
        {
            if (_requireCards.Count == 0)
            {
                return true;
            }

            bool result = _requireCards.TrueForAll(requireCard => requireCard.CheckHeroes());
            return result;
        }

        public void ClearData()
        {
            foreach (RequireCard requireCard in _requireCards)
            {
                requireCard.ClearData();
            }
        }

        private void OnDestroy()
        {
            _disposables?.Dispose();
        }
    }
}