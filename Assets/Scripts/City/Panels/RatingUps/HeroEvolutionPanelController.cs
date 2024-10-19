using City.Panels.RatingUps;
using City.Panels.RatingUps.EvolutionResultPanels;
using City.TrainCamp.HeroInstances;
using ClientServices;
using Common.Heroes;
using Common.Resourses;
using Cysharp.Threading.Tasks;
using Db.CommonDictionaries;
using Hero;
using Misc.Json;
using Models.City.TrainCamp;
using Models.Heroes.PowerUps;
using Network.DataServer;
using Network.DataServer.Messages.HeroPanels;
using System;
using System.Collections.Generic;
using UIController;
using UiExtensions.Scroll.Interfaces;
using UniRx;
using VContainer;

namespace City.TrainCamp
{
    public class HeroEvolutionPanelController : UiPanelController<HeroEvolutionPanelView>
    {
        [Inject] private readonly HeroesStorageController _heroesStorageController;
        [Inject] private readonly HeroPanelController _heroPanelController;
        [Inject] private readonly ResourceStorageController _resourceStorageController;
        [Inject] private readonly CommonDictionaries _commonDictionaries;
        [Inject] private readonly IJsonConverter _jsonConverter;
        [Inject] private readonly HeroInstancesController _heroInstancesController;
        [Inject] private readonly EvolutionResultPanelController _evolutionResultPanelController;

        private GameHero _currentHero;
        private RatingUpContainer _data;
        private List<GameResource> _cost = new();
        private ReactiveCommand<GameHero> _onRatingUp = new();

        public IObservable<GameHero> OnRatingUp => _onRatingUp;

        public override void Start()
        {
            View.ListRequirementHeroes.MainController = this;
            View.ButtonLevelUP.OnClickAsObservable().Subscribe(_ => RatingUp()).AddTo(Disposables);
            View.CardsContainer.OnSelect.Subscribe(SelectHero).AddTo(Disposables);
            View.CardsContainer.OnDiselect.Subscribe(UnselectHero).AddTo(Disposables);
            View.DimmedSelectedPanelButton.OnClickAsObservable().Subscribe(_ => CloseSelectedPanel()).AddTo(Disposables);
            base.Start();

            View.ListRequirementHeroes.OnSelectRequireCard.Subscribe(SelectRequireCard).AddTo(Disposables);

            foreach (var requireCard in View.ListRequirementHeroes.RequireCards)
            {
                Resolver.Inject(requireCard.Card);
            }
            Resolver.Inject(View.RequireCardInfo.Card);
            Resolver.Inject(View.CardsContainer);

            View.SelectHeroesPanel.SetActive(false);
        }

        public override void OnShow()
        {
            _currentHero = _heroPanelController.Hero;
            _data = _commonDictionaries.RatingUpContainers[$"{_currentHero.HeroData.Rating + 1}"];
            View.ListRequirementHeroes.SetData(_currentHero, _data.RequirementHeroes);

            _cost.Clear();
            _cost = new List<GameResource>(_data.Cost.Count);
            foreach (var resource in _data.Cost)
                _cost.Add(new GameResource(resource));

            View.CostController.ShowCosts(_cost);
            CheckCanUpdateRating();
            _heroInstancesController.ShowHero(_currentHero);
            View.RatingHeroController.ShowRating(_currentHero.HeroData.Rating);
        }

        private void SelectRequireCard(RequireCard card)
        {
            var currentHeroes = _heroesStorageController.ListHeroes;
            currentHeroes = currentHeroes.FindAll(hero => CheckÑonformity(hero, card.RequirementHero));
            currentHeroes.Remove(_currentHero);

            View.CardsContainer.ShowCards(currentHeroes);
            View.CardsContainer.SelectCards(card.SelectedHeroes);
            View.SelectHeroesPanel.SetActive(true);
            View.RequireCardInfo.SetData(_currentHero, card.RequirementHero);
            View.RequireCardInfo.SetProgress(card.RequirementHero, card.SelectedHeroes);
            View.RequireCardInfo.OnOpenList();
        }

        private void CloseSelectedPanel()
        {
            foreach (var requireCard in View.ListRequirementHeroes.RequireCards)
            {
                requireCard.OnCloseListCard();
            }
            View.RequireCardInfo.OnCloseListCard();

            View.SelectHeroesPanel.SetActive(false);
            CheckCanUpdateRating();

        }

        private bool CheckÑonformity(GameHero hero, RequirementHeroModel requirementHero)
        {
            if (!hero.Model.General.Race.Equals(_currentHero.Model.General.Race))
                return false;

            if (!hero.HeroData.Rating.Equals(requirementHero.Rating))
                return false;

            return true;
        }

        public void SelectHero(GameHero hero)
        {
            var selectedCard = View.CardsContainer.Cards.Find(card => card.Hero == hero);
            selectedCard.Select();
        }

        public void UnselectHero(GameHero hero)
        {
            var selectedCard = View.CardsContainer.Cards.Find(card => card.Hero == hero);
            selectedCard.Unselect();
        }

        public void RatingUp()
        {
            HeroRatingUp().Forget();
        }

        public void CheckCanUpdateRating()
        {
            var resourceDone = _resourceStorageController.CheckResource(_cost);
            var requireHeroesDone = View.ListRequirementHeroes.IsAllDone();
            View.ButtonLevelUP.interactable = resourceDone && requireHeroesDone;
        }

        public void CheckResource(GameResource res)
        {
            CheckCanUpdateRating();
        }

        public void CheckHeroes()
        {
            CheckCanUpdateRating();
        }

        protected void ClosePage()
        {
            View.ListRequirementHeroes.ClearData();
        }

        private async UniTaskVoid HeroRatingUp()
        {
            var heroesPayment = new List<int>(View.ListRequirementHeroes.SelectedHeroes.Count);

            foreach (var requireCard in View.ListRequirementHeroes.RequireCards)
            {
                foreach (var hero in requireCard.SelectedHeroes)
                {
                    heroesPayment.Add(hero.HeroData.Id);
                }
                _heroesStorageController.RemoveHeroes(requireCard.SelectedHeroes);
            }

            var message = new HeroRatingUpMessage
            {
                PlayerId = CommonGameData.PlayerInfoData.Id,
                HeroId = _currentHero.HeroData.Id,
                HeroesPaymentContainer = _jsonConverter.Serialize(heroesPayment)
            };

            var result = await DataServer.PostData(message);

            if (!string.IsNullOrEmpty(result))
            {
                _resourceStorageController.SubtractResource(_cost);

                foreach (var requireCard in View.ListRequirementHeroes.RequireCards)
                    _heroesStorageController.RemoveHeroes(requireCard.SelectedHeroes);

                Close();
                _evolutionResultPanelController.OpenEvolvedHero(_currentHero);

                _onRatingUp.Execute(_currentHero);

                _heroPanelController.UpdateInfoAboutHero();
            }
        }

        protected override void Close()
        {
            _heroInstancesController.Hide();
            _heroPanelController.ShowHero(_currentHero);
            _cost.Clear();
            base.Close();
        }
    }
}