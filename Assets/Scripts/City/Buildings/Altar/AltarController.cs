using System;
using System.Collections.Generic;
using City.Buildings.Abstractions;
using City.Panels.AltarMarkets;
using ClientServices;
using Common.Db.CommonDictionaries;
using Common.Heroes;
using Common.Rewards;
using Cysharp.Threading.Tasks;
using Hero;
using Misc.Json;
using Models.Common.BigDigits;
using Network.DataServer;
using Network.DataServer.Messages;
using Network.DataServer.Models;
using UIController.Cards;
using UIController.Rewards;
using UniRx;
using VContainer;
using VContainer.Unity;
using VContainerUi.Messages;
using VContainerUi.Model;
using VContainerUi.Services;

namespace City.Buildings.Altar
{
    public class AltarController : BuildingWithHeroesList<AltarView>, IInitializable
    {
        [Inject] private readonly HeroesStorageController m_heroesStorageController;
        [Inject] private readonly IJsonConverter m_jsonConverter;
        [Inject] private readonly ClientRewardService m_clientRewardService;
        [Inject] private readonly CommonDictionaries m_commonDictionaries;
        [Inject] protected readonly IUiMessagesPublisherService UiMessagesPublisher;

        private readonly ReactiveCommand<BigDigit> m_onDissamble = new ReactiveCommand<BigDigit>();

        private List<GameHero> m_listHeroes;
        private readonly List<Card> m_selectedHeroCards = new();

        public IObservable<BigDigit> OnDissamble => m_onDissamble;

        public void Initialize()
        {
            View.CardsContainer.OnSelect.Subscribe(SelectHero).AddTo(Disposables);
            View.CardsContainer.OnDiselect.Subscribe(UnselectHero).AddTo(Disposables);
            View.MusterOutButton.OnClickAsObservable().Subscribe(_ => FiredHeroes().Forget()).AddTo(Disposables);
            View.AltarMarketButton.OnClickAsObservable().Subscribe(_ => OpenAltarMarket()).AddTo(Disposables);
            Resolver.Inject(View.CardsContainer);

            View.MusterOutButton.interactable = false;

		}

        protected override void OnLoadGame()
        {
            m_listHeroes = m_heroesStorageController.ListHeroes;
        }

        public override void OnShow()
        {
            View.CardsContainer.ShowCards(m_listHeroes);
            base.OnShow();
        }

        private void OpenAltarMarket()
        {
            UiMessagesPublisher.OpenWindowPublisher.OpenWindow<AltarMarketPageController>(openType: OpenType.Additive);
        }
        
        private async UniTaskVoid FiredHeroes()
        {
            FireContainer fireContainer = new FireContainer();

            foreach (var card in m_selectedHeroCards)
            {
                fireContainer.HeroesIds.Add(card.Hero.HeroData.Id);
            }

            FireHeroesMessage message = new FireHeroesMessage
            {
                PlayerId = CommonGameData.PlayerInfoData.Id,
                Container = fireContainer
            };
            
            string result = await DataServer.PostData(message);

            RewardModel rewardModel = m_jsonConverter.Deserialize<RewardModel>(result);
            GameReward reward = new GameReward(rewardModel, m_commonDictionaries);
            m_clientRewardService.ShowReward(reward);

            List<GameHero> heroes = new List<GameHero>();

            m_onDissamble.Execute(new BigDigit(m_selectedHeroCards.Count));

            foreach (Card card in m_selectedHeroCards)
            {
                heroes.Add(card.Hero);
                card.Unselect();
            }
            
            View.CardsContainer.RemoveCards(m_selectedHeroCards);
            m_selectedHeroCards.Clear();

            foreach (GameHero hero in heroes)
            {
                m_heroesStorageController.RemoveHero(hero);
            }
        }

        private void SelectHero(GameHero hero)
        {
            var selectedCard = View.CardsContainer.Cards.Find(card => card.Hero == hero);
            m_selectedHeroCards.Add(selectedCard);
            selectedCard.Select();

			View.MusterOutButton.interactable = m_selectedHeroCards.Count > 0;
		}

        private void UnselectHero(GameHero hero)
        {
            var selectedCard = View.CardsContainer.Cards.Find(card => card.Hero == hero);
            m_selectedHeroCards.Remove(selectedCard);
            selectedCard.Unselect();

			View.MusterOutButton.interactable = m_selectedHeroCards.Count > 0;
		}
    }
}