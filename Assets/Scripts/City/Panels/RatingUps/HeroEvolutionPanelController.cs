using System;
using System.Collections.Generic;
using City.Panels.RatingUps.EvolutionResultPanels;
using City.TrainCamp.HeroInstances;
using City.TrainCamp.HeroPanels;
using ClientServices;
using Common.Db.CommonDictionaries;
using Common.Heroes;
using Common.Inventories.Resourses;
using Cysharp.Threading.Tasks;
using Hero;
using LocalizationSystems;
using Misc.Json;
using Models.City.TrainCamp;
using Models.Data.Inventories;
using Models.Heroes.PowerUps;
using Network.DataServer;
using Network.DataServer.Messages.HeroPanels;
using UIController;
using UIController.Cards;
using UiExtensions.Panels;
using UniRx;
using VContainer;

namespace City.Panels.RatingUps
{
    public class HeroEvolutionPanelController : UiPanelController<HeroEvolutionPanelView>
    {
        [Inject] private readonly HeroesStorageController m_heroesStorageController;
        [Inject] private readonly HeroPanelController m_heroPanelController;
        [Inject] private readonly ResourceStorageController m_resourceStorageController;
        [Inject] private readonly CommonDictionaries m_commonDictionaries;
        [Inject] private readonly IJsonConverter m_jsonConverter;
        [Inject] private readonly HeroInstancesController m_heroInstancesController;
        [Inject] private readonly EvolutionResultPanelController m_evolutionResultPanelController;
        [Inject] private readonly ILocalizationSystem m_localizationSystem;

        private HeroRenderContainer m_oldHeroRenderContainer;
        private HeroRenderContainer m_newHeroRenderContainer;
        private GameHero m_currentHero;
        private RatingUpContainer m_data;
        private List<GameResource> m_cost = new();
        private readonly ReactiveCommand<GameHero> m_onRatingUp = new();

        public IObservable<GameHero> OnRatingUp => m_onRatingUp;

        public override void Start()
        {
            View.ListRequirementHeroes.MainController = this;
            View.ButtonLevelUP.OnClickAsObservable().Subscribe(_ => RatingUp()).AddTo(Disposables);
            View.CardsContainer.OnSelect.Subscribe(SelectHero).AddTo(Disposables);
            View.CardsContainer.OnDiselect.Subscribe(UnselectHero).AddTo(Disposables);
            View.DimmedSelectedPanelButton.OnClickAsObservable().Subscribe(_ => CloseSelectedPanel())
                .AddTo(Disposables);
            base.Start();

            View.ListRequirementHeroes.OnSelectRequireCard.Subscribe(SelectRequireCard).AddTo(Disposables);

            foreach (RequireCard requireCard in View.ListRequirementHeroes.RequireCards)
                Resolver.Inject(requireCard.Card);
            Resolver.Inject(View.RequireCardInfo.Card);
            Resolver.Inject(View.CardsContainer);

            View.SelectHeroesPanel.SetActive(false);
        }

        public override void OnShow()
        {
            m_currentHero = m_heroPanelController.Hero;
            m_data = m_commonDictionaries.RatingUpContainers[$"{m_currentHero.HeroData.Rating + 1}"];
            View.ListRequirementHeroes.SetData(m_currentHero, m_data.RequirementHeroes);

            m_cost.Clear();
            m_cost = new List<GameResource>(m_data.Cost.Count);
            foreach (ResourceData resource in m_data.Cost)
                m_cost.Add(new GameResource(resource));

            View.CostController.ShowCosts(m_cost);
            CheckCanUpdateRating();
            
            ReleaseRenders();
            m_oldHeroRenderContainer = m_heroInstancesController.ShowHero(m_currentHero);
            m_newHeroRenderContainer = m_heroInstancesController.ShowHero(m_currentHero, 1);
            
            View.OldHeroRawImage.texture = m_oldHeroRenderContainer.RenderTexture;
            View.NewHeroRawImage.texture = m_newHeroRenderContainer.RenderTexture;
            
            View.OldRatingHeroController.ShowRating(m_currentHero.HeroData.Rating);
            View.NewRatingHeroController.ShowRating(m_currentHero.HeroData.Rating + 1);
            
            View.NameText.StringReference = m_localizationSystem
                .GetLocalizedContainer($"Hero{m_currentHero.Model.General.HeroId}Name");
        }

        private void SelectRequireCard(RequireCard card)
        {
            List<GameHero> currentHeroes = m_heroesStorageController.ListHeroes;
            currentHeroes = currentHeroes.FindAll(hero => CheckConformity(hero, card.RequirementHero));
            currentHeroes.Remove(m_currentHero);

            View.CardsContainer.ShowCards(currentHeroes);
            View.CardsContainer.SelectCards(card.SelectedHeroes);
            View.SelectHeroesPanel.SetActive(true);
            View.RequireCardInfo.SetData(m_currentHero, card.RequirementHero);
            View.RequireCardInfo.SetProgress(card.RequirementHero, card.SelectedHeroes);
            View.RequireCardInfo.OnOpenList();
        }

        private void CloseSelectedPanel()
        {
            foreach (RequireCard requireCard in View.ListRequirementHeroes.RequireCards) requireCard.OnCloseListCard();
            View.RequireCardInfo.OnCloseListCard();

            View.SelectHeroesPanel.SetActive(false);
            CheckCanUpdateRating();
        }

        private bool CheckConformity(GameHero hero, RequirementHeroModel requirementHero)
        {
            if (!hero.Model.General.Race.Equals(m_currentHero.Model.General.Race))
                return false;

            if (!hero.HeroData.Rating.Equals(requirementHero.Rating))
                return false;

            return true;
        }

        private void SelectHero(GameHero hero)
        {
            Card selectedCard = View.CardsContainer.Cards.Find(card => card.Hero == hero);
            selectedCard.Select();
        }

        private void UnselectHero(GameHero hero)
        {
            Card selectedCard = View.CardsContainer.Cards.Find(card => card.Hero == hero);
            selectedCard.Unselect();
        }

        private void RatingUp()
        {
            HeroRatingUp().Forget();
        }

        private void CheckCanUpdateRating()
        {
            bool resourceDone = m_resourceStorageController.CheckResource(m_cost);
            bool requireHeroesDone = View.ListRequirementHeroes.IsAllDone();
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
            List<int> heroesPayment = new List<int>(View.ListRequirementHeroes.SelectedHeroes.Count);

            foreach (RequireCard requireCard in View.ListRequirementHeroes.RequireCards)
            {
                foreach (GameHero hero in requireCard.SelectedHeroes) heroesPayment.Add(hero.HeroData.Id);
                m_heroesStorageController.RemoveHeroes(requireCard.SelectedHeroes);
            }

            HeroRatingUpMessage message = new HeroRatingUpMessage
            {
                PlayerId = CommonGameData.PlayerInfoData.Id,
                HeroId = m_currentHero.HeroData.Id,
                HeroesPaymentContainer = m_jsonConverter.Serialize(heroesPayment)
            };

            string result = await DataServer.PostData(message);

            if (!string.IsNullOrEmpty(result))
            {
                m_resourceStorageController.SubtractResource(m_cost);

                foreach (RequireCard requireCard in View.ListRequirementHeroes.RequireCards)
                    m_heroesStorageController.RemoveHeroes(requireCard.SelectedHeroes);

                Close();
                m_evolutionResultPanelController.OpenEvolvedHero(m_currentHero);

                m_onRatingUp.Execute(m_currentHero);

                m_heroPanelController.UpdateInfoAboutHero();
            }
        }

        protected override void Close()
        {
            ReleaseRenders();
            m_heroPanelController.ShowHero(m_currentHero);
            m_cost.Clear();
            base.Close();
        }

        private void ReleaseRenders()
        {
            if (m_oldHeroRenderContainer != null)
            {
                m_heroInstancesController.ReleaseRender(m_oldHeroRenderContainer);
                m_oldHeroRenderContainer = null;
            }
            
            if (m_newHeroRenderContainer != null)
            {
                m_heroInstancesController.ReleaseRender(m_newHeroRenderContainer);
                m_newHeroRenderContainer = null;
            }
        }
    }
}