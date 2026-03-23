using System.Collections.Generic;
using System.Linq;
using City.Buildings.Abstractions;
using City.Panels.HeroesHireResultPanels;
using ClientServices;
using Common.Db.CommonDictionaries;
using Common.Heroes;
using Common.Inventories.Resourses;
using Common.Resourses;
using Cysharp.Threading.Tasks;
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
        [Inject] private readonly HeroesStorageController m_heroesStorageController;
        [Inject] private readonly ResourceStorageController m_resourceStorageController;
        [Inject] private readonly CommonGameData m_сommonGameData;
        [Inject] private readonly CommonDictionaries m_commonDictionaries;
        [Inject] private readonly IJsonConverter m_jsonConverter;
        [Inject] private readonly HeroesHireResultPanelController m_heroesHireResultPanelController;

        private Card m_selectedCard;
        private List<GameHero> m_listHeroes;

        private ResourceType m_defaultResourceType;
        private string m_targetRace = "Random";

        private SanctuaryBuildingModel m_sanctuaryBuildingModel;

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
            m_sanctuaryBuildingModel =
                m_commonDictionaries.Buildings[nameof(SanctuaryBuildingModel)] as SanctuaryBuildingModel;
            
            if (m_sanctuaryBuildingModel == null)
            {
                return;
            }

            m_listHeroes = m_heroesStorageController.ListHeroes;
            
            var costReplaceData = m_sanctuaryBuildingModel.SimpleReplaceResource[0];
            m_defaultResourceType = costReplaceData.Type;
            View.ReplacementButton.ChangeCost(new GameResource(m_defaultResourceType), null);
            View.ReplacementButton.buttonCostComponent.Disable();
        }

        private void ChangeTargetRace(string targetRace)
        {
            View.RaceButtons[m_targetRace].interactable = true;
            m_targetRace = targetRace;
            View.RaceButtons[m_targetRace].interactable = false;

            if (m_selectedCard == null) return;

            SetCostReplace(m_selectedCard);
        }

        public override void OnShow()
        {
            RefreshList();

            base.OnShow();
        }

        private async UniTaskVoid ReplacementHero(GameResource costReplace)
        {
            if (m_selectedCard != null)
                if (m_resourceStorageController.CheckResource(costReplace))
                {
                    View.ReplacementButton.buttonCostComponent.Disable();
                    var message = new ReplaceHeroMessage
                    {
                        PlayerId = CommonGameData.PlayerInfoData.Id,
                        HeroId = m_selectedCard.Hero.HeroData.Id,
                        TargetRace = m_targetRace
                    };

                    var result = await DataServer.PostData(message);

                    if (!string.IsNullOrEmpty(result))
                    {
                        View.CardsContainer.RemoveCard(m_selectedCard);
                        m_heroesStorageController.RemoveHero(m_selectedCard.Hero);

                        var heroData = m_jsonConverter.Deserialize<HeroData>(result);

                        var model = m_commonDictionaries.Heroes[heroData.HeroId];
                        var hero = new GameHero(model, heroData);
                        m_heroesHireResultPanelController.ShowHeroes(new List<GameHero>() { hero });

                        m_selectedCard = null;
                        m_heroesStorageController.AddHero(hero);

                        View.ReplacementButton.ChangeCost(new GameResource(m_defaultResourceType), null);
                        m_resourceStorageController.SubtractResource(costReplace);
                        RefreshList();
                    }
                }
        }

        private void SelectHero(GameHero hero)
        {
            if (m_selectedCard != null) m_selectedCard.Unselect();

            m_selectedCard = View.CardsContainer.Cards.Find(card => card.Hero == hero);
            m_selectedCard.Select();

            SetCostReplace(m_selectedCard);
        }

        private void RefreshList()
        {
            m_listHeroes = m_heroesStorageController.ListHeroes
                .Where(hero => hero.HeroData.Rating <= 5)
                .ToList();

            View.CardsContainer.ShowCards(m_listHeroes);
        }
        
        private void SetCostReplace(Card selectedCard)
        {
            ResourceData costReplaceData = null;

            if (m_targetRace == "Random")
                costReplaceData = m_sanctuaryBuildingModel.SimpleReplaceResource[selectedCard.Hero.HeroData.Rating - 1];
            else
                costReplaceData =
                    m_sanctuaryBuildingModel.ConcreteReplaceResource[selectedCard.Hero.HeroData.Rating - 1];

            var costReplace = new GameResource(costReplaceData);

            View.ReplacementButton.buttonCostComponent.Enable();
            View.ReplacementButton
                .ChangeCost(costReplace, () => ReplacementHero(costReplace).Forget());
        }

        public void UnselectHero(GameHero hero)
        {
            var selectedCard = View.CardsContainer.Cards.Find(card => card.Hero == hero);
            selectedCard.Unselect();
            m_selectedCard = null;

            View.ReplacementButton.ChangeCost(new GameResource(m_defaultResourceType), null);
            View.ReplacementButton.buttonCostComponent.Disable();
        }
    }
}