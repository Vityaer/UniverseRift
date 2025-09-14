using System.Collections.Generic;
using System.Linq;
using City.Buildings.Abstractions;
using City.Panels.HeroesHireResultPanels;
using ClientServices;
using Common.Heroes;
using Common.Resourses;
using Cysharp.Threading.Tasks;
using Db.CommonDictionaries;
using Hero;
using Misc.Json;
using Models;
using Models.City.Sanctuaries;
using Models.Common;
using Models.Data.Inventories;
using Network.DataServer;
using Network.DataServer.Messages;
using UIController.Cards;
using UniRx;
using VContainer;
using VContainer.Unity;

namespace City.Buildings.Sanctuary
{
    public class SanctuaryController : BuildingWithHeroesList<SanctuaryView>, IInitializable
    {
        [Inject] private readonly HeroesStorageController _heroesStorageController;
        [Inject] private readonly ResourceStorageController _resourceStorageController;
        [Inject] private readonly CommonGameData _сommonGameData;
        [Inject] private readonly CommonDictionaries _commonDictionaries;
        [Inject] private readonly IJsonConverter _jsonConverter;
        [Inject] private readonly HeroesHireResultPanelController _heroesHireResultPanelController;

        private Card _selectedCard;
        private List<GameHero> _listHeroes;

        private ResourceType m_defaultResourceType;
        private string m_targetRace = "Random";

        private SanctuaryBuildingModel _sanctuaryBuildingModel;

        public void Initialize()
        {
            View.CardsContainer.OnSelect.Subscribe(SelectHero).AddTo(Disposables);
            View.CardsContainer.OnDiselect.Subscribe(UnselectHero).AddTo(Disposables);

            foreach (var raceButton in View.RaceButtons)
                raceButton.Value.OnClickAsObservable()
                    .Subscribe(_ => ChangeTargetRace(raceButton.Key))
                    .AddTo(Disposables);

            View.RaceButtons[m_targetRace].interactable = false;
            Resolver.Inject(View.CardsContainer);
        }

        protected override void OnLoadGame()
        {
            _sanctuaryBuildingModel =
                _commonDictionaries.Buildings[nameof(SanctuaryBuildingModel)] as SanctuaryBuildingModel;
            
            if (_sanctuaryBuildingModel == null)
            {
                return;
            }

            _listHeroes = _heroesStorageController.ListHeroes;
            
            var costReplaceData = _sanctuaryBuildingModel.SimpleReplaceResource[0];
            m_defaultResourceType = costReplaceData.Type;
            View.ReplacementButton.ChangeCost(new GameResource(m_defaultResourceType), null);
            View.ReplacementButton.buttonCostComponent.Disable();
        }

        private void ChangeTargetRace(string targetRace)
        {
            View.RaceButtons[m_targetRace].interactable = true;
            m_targetRace = targetRace;
            View.RaceButtons[m_targetRace].interactable = false;

            if (_selectedCard == null) return;

            SetCostReplace(_selectedCard);
        }

        public override void OnShow()
        {
            _listHeroes = _heroesStorageController.ListHeroes
                .Where(hero => hero.HeroData.Rating <= 5)
                .ToList();

            View.CardsContainer.ShowCards(_listHeroes);
            base.OnShow();
        }

        private async UniTaskVoid ReplacementHero(GameResource costReplace)
        {
            if (_selectedCard != null)
                if (_resourceStorageController.CheckResource(costReplace))
                {
                    View.ReplacementButton.buttonCostComponent.Disable();
                    var message = new ReplaceHeroMessage
                    {
                        PlayerId = CommonGameData.PlayerInfoData.Id,
                        HeroId = _selectedCard.Hero.HeroData.Id,
                        TargetRace = m_targetRace
                    };

                    var result = await DataServer.PostData(message);

                    if (!string.IsNullOrEmpty(result))
                    {
                        View.CardsContainer.RemoveCard(_selectedCard);
                        _heroesStorageController.RemoveHero(_selectedCard.Hero);

                        var heroData = _jsonConverter.Deserialize<HeroData>(result);

                        var model = _commonDictionaries.Heroes[heroData.HeroId];
                        var hero = new GameHero(model, heroData);
                        _heroesHireResultPanelController.ShowHeroes(new List<GameHero>() { hero });

                        _selectedCard = null;

                        View.ReplacementButton.ChangeCost(new GameResource(m_defaultResourceType), null);
                        _resourceStorageController.SubtractResource(costReplace);
                        View.CardsContainer.ShowCards(_listHeroes);
                    }
                }
        }

        public void SelectHero(GameHero hero)
        {
            if (_selectedCard != null) _selectedCard.Unselect();

            _selectedCard = View.CardsContainer.Cards.Find(card => card.Hero == hero);
            _selectedCard.Select();

            SetCostReplace(_selectedCard);
        }

        private void SetCostReplace(Card selectedCard)
        {
            ResourceData costReplaceData = null;

            if (m_targetRace == "Random")
                costReplaceData = _sanctuaryBuildingModel.SimpleReplaceResource[selectedCard.Hero.HeroData.Rating - 1];
            else
                costReplaceData =
                    _sanctuaryBuildingModel.ConcreteReplaceResource[selectedCard.Hero.HeroData.Rating - 1];

            var costReplace = new GameResource(costReplaceData);

            View.ReplacementButton.buttonCostComponent.Enable();
            View.ReplacementButton
                .ChangeCost(costReplace, () => ReplacementHero(costReplace).Forget());
        }

        public void UnselectHero(GameHero hero)
        {
            var selectedCard = View.CardsContainer.Cards.Find(card => card.Hero == hero);
            selectedCard.Unselect();
            _selectedCard = null;

            View.ReplacementButton.ChangeCost(new GameResource(m_defaultResourceType), null);
            View.ReplacementButton.buttonCostComponent.Disable();
        }
    }
}