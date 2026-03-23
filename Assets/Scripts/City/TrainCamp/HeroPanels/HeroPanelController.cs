using System;
using System.Collections.Generic;
using System.Threading;
using City.Panels.RatingUps;
using City.SliderCity;
using City.TrainCamp.HeroInstances;
using City.TrainCamp.HeroPanels.HeroDetails;
using ClientServices;
using Common.Db.CommonDictionaries;
using Common.Inventories.Resourses;
using Cysharp.Threading.Tasks;
using Hero;
using LocalizationSystems;
using Misc.Json;
using Models.Common;
using Models.Heroes.HeroCharacteristics;
using Network.DataServer;
using Network.DataServer.Messages.HeroPanels;
using UIController.ItemVisual;
using UIController.SkillPanels;
using UiExtensions.Panels;
using UniRx;
using UniRx.Triggers;
using UnityEngine;
using Utils.AsyncUtils;
using VContainer;
using VContainer.Unity;
using VContainerUi.Messages;
using VContainerUi.Model;

namespace City.TrainCamp.HeroPanels
{
    public class HeroPanelController : UiPanelController<HeroPanelView>, IInitializable
    {
        [Inject] private readonly CommonDictionaries m_dictionaries;
        [Inject] private readonly CommonGameData m_commonGameData;
        [Inject] private readonly ResourceStorageController m_resourceStorageController;
        [Inject] private readonly HeroInstancesController m_heroInstancesController;
        [Inject] private readonly HeroDetailsPanelController m_heroDetailsPanel;
        [Inject] private readonly IObjectResolver m_objectResolver;
        [Inject] private readonly IJsonConverter m_jsonConverter;
        [Inject] private readonly SkillPanelController m_skillPanelController;
        [Inject] private readonly ILocalizationSystem m_localizationSystem;

        private readonly ReactiveCommand<GameHero> m_onLevelUpCommand = new();
        private readonly ReactiveCommand<SwipeType> m_onSwipe = new();
        
        private HeroRenderContainer m_heroRenderContainer;
        private CostLevelUpContainer m_costLevelObject;
        private GameHero m_currentHero;

        public GameHero Hero => m_currentHero;
        public IObservable<SwipeType> OnSwipe => m_onSwipe;
        public IObservable<GameHero> OnLevelUp => m_onLevelUpCommand;

        public new void Initialize()
        {
            base.Initialize();
            View.CostController.InjectAll(m_objectResolver);

            foreach (HeroItemCellController cell in View.CellsForItem) m_objectResolver.Inject(cell);

            foreach (SkillCell cell in View.SkillCells)
                cell.OnSelect.Subscribe(OpenSkillDetails).AddTo(Disposables);

            View.EvolitionPanelButton.OnClickAsObservable().Subscribe(_ => OpenEvolutionPanel()).AddTo(Disposables);
            View.ToLeftButton.OnClickAsObservable().Subscribe(_ => m_onSwipe.Execute(SwipeType.Left))
                .AddTo(Disposables);
            View.ToRightButton.OnClickAsObservable().Subscribe(_ => m_onSwipe.Execute(SwipeType.Right))
                .AddTo(Disposables);
            View.LevelUpButton.OnPointerDownAsObservable().Subscribe(_ => StartLevelUp()).AddTo(Disposables);
            View.LevelUpButton.OnPointerUpAsObservable().Subscribe(_ => FinishLevelUp()).AddTo(Disposables);
            View.OpenHeroDetailsButton.OnClickAsObservable().Subscribe(_ => DetailsOpen()).AddTo(Disposables);
        }



        private void OpenSkillDetails(SkillCell cell)
        {
            m_skillPanelController.ShowSkillData(m_currentHero, cell.Data);
        }

        protected override void OnLoadGame()
        {
            m_costLevelObject = m_dictionaries.CostContainers["Heroes"];
        }

        private void DetailsOpen()
        {
            m_heroDetailsPanel.SetData(m_currentHero);
            MessagesPublisher.OpenWindowPublisher.OpenWindow<HeroDetailsPanelController>(openType: OpenType.Additive);
        }

        public void ShowHero(GameHero hero)
        {
            m_currentHero = hero;
            UpdateInfoAboutHero();

            if (m_heroRenderContainer != null) m_heroInstancesController.ReleaseRender(m_heroRenderContainer);

            m_heroRenderContainer = m_heroInstancesController.ShowHero(hero);
            View.HeroRawImage.texture = m_heroRenderContainer.RenderTexture;
        }


        public void ShowLeftArrow(bool flag)
        {
            View.ToLeftButton.gameObject.SetActive(flag);
        }

        public void ShowRightArrow(bool flag)
        {
            View.ToRightButton.gameObject.SetActive(flag);
        }

        public void UpdateInfoAboutHero()
        {
            View.NameHero.StringReference = m_localizationSystem
                .GetLocalizedContainer($"Hero{m_currentHero.Model.General.HeroId}Name");

            UpdateTextAboutHero();
            foreach (HeroItemCellController cell in View.CellsForItem)
            {
                cell.Clear();
                cell.SetData(m_currentHero.Costume.GetItem(cell.CellType));
            }

            CheckControllers();
            View.RatingHeroController.ShowRating(m_currentHero.HeroData.Rating);
        }

        public void UpdateTextAboutHero()
        {
            View.textLevel.text = $"{m_currentHero.HeroData.Level}";
            View.textHP.text = ((int)m_currentHero.GetCharacteristic(m_dictionaries, TypeCharacteristic.HP)).ToString();
            View.textAttack.text = ((int)m_currentHero.GetCharacteristic(m_dictionaries, TypeCharacteristic.Damage))
                .ToString();
            View.textArmor.text = ((int)m_currentHero.GetCharacteristic(m_dictionaries, TypeCharacteristic.Defense))
                .ToString();
            View.textInitiative.text =
                ((int)m_currentHero.GetCharacteristic(m_dictionaries, TypeCharacteristic.Initiative)).ToString();
            View.textStrengthHero.text = m_currentHero.CalculateStrength(m_dictionaries).ToString();
            ShowSkills();
            View.CostController.ShowCosts(m_costLevelObject.GetCostForLevelUp(m_currentHero.HeroData.Level));
        }

        private void ShowSkills()
        {
            for (int i = 0; i < m_currentHero.Model.Skills.Count && i < View.SkillCells.Count; i++)
                View.SkillCells[i].SetData(m_currentHero.Model.Skills[i]);

            for (int i = m_currentHero.Model.Skills.Count; i < View.SkillCells.Count; i++)
                View.SkillCells[i].OffObject();
        }

        private void CheckControllers()
        {
            string nextRaitingContainerId = $"{m_currentHero.HeroData.Rating + 1}";
            View.EvolitionPanelButton.gameObject
                .SetActive(m_dictionaries.RatingUpContainers.ContainsKey(nextRaitingContainerId));

            int heroLevel = m_currentHero.HeroData.Level;
            int heroBreakthrough = m_currentHero.HeroData.CurrentBreakthrough;
            int limitLevel = heroLevel;

            if (heroBreakthrough < m_currentHero.Model.Evolutions.Stages.Count)
                limitLevel = m_currentHero.Model.Evolutions.Stages[heroBreakthrough].NewLimitLevel;

            bool enoughResource = m_resourceStorageController
                .CheckResource(m_costLevelObject.GetCostForLevelUp(m_currentHero.HeroData.Level));

            View.LevelUpButton.gameObject.SetActive(heroLevel < limitLevel);
            View.LevelUpButton.interactable = enoughResource;
        }

        private bool m_levelUpMessaging;
        private CancellationTokenSource m_cancellationTokenSource;
        
        private void StartLevelUp()
        {
            if (!View.LevelUpButton.interactable)
            {
                return;
            }

            if (m_cancellationTokenSource != null)
            {
                return;
            }
            
            m_cancellationTokenSource = new CancellationTokenSource();
            LevelUping(m_cancellationTokenSource.Token).Forget();
        }

        private async UniTask LevelUping(CancellationToken cancellationToken)
        {
            while (!cancellationToken.IsCancellationRequested)
            {
                if (m_levelUpMessaging)
                {
                    await UniTask.Delay(TimeSpan.FromSeconds(View.LevelUpRequestDelay),cancellationToken: cancellationToken);
                    continue;
                }

                if (!View.LevelUpButton.interactable)
                {
                    FinishLevelUp();
                    break;
                }

                LevelUp();
                await UniTask.Delay(TimeSpan.FromSeconds(View.LevelUpRequestDelay),cancellationToken: cancellationToken);
            }
        }

        private void FinishLevelUp()
        {
            m_cancellationTokenSource.TryCancel();
            m_cancellationTokenSource = null;
        }
        
        private void LevelUp()
        {
            try
            {
                List<GameResource> cost = m_costLevelObject.GetCostForLevelUp(m_currentHero.HeroData.Level);
                if (!m_resourceStorageController.CheckResource(cost))
                {
                    return;
                }

                LevelUpMessage(m_currentHero).Forget();
                m_resourceStorageController.SubtractResource(cost);
                m_currentHero.LevelUp();
                UpdateInfoAboutHero();
                m_heroRenderContainer.ShowAnimation();
            }
            catch(Exception ex)
            {
                Debug.LogError(ex);
            }
        }

        private async UniTaskVoid LevelUpMessage(GameHero levelUpHero)
        {
            m_levelUpMessaging = true;
            HeroLevelUpMessage message = new HeroLevelUpMessage()
                { PlayerId = m_commonGameData.PlayerInfoData.Id, HeroId = m_currentHero.HeroData.Id };
            string result = await DataServer.PostData(message);
            
            if (!string.IsNullOrEmpty(result))
            {
                m_onLevelUpCommand.Execute(levelUpHero);
            }
            m_levelUpMessaging = false;
        }

        private void OpenEvolutionPanel()
        {
            MessagesPublisher.OpenWindowPublisher.OpenWindow<HeroEvolutionPanelController>(openType: OpenType.Additive);
        }

        private void ReleaseRender()
        {
            if (m_heroRenderContainer == null)
                return;

            m_heroInstancesController.ReleaseRender(m_heroRenderContainer);
            m_heroRenderContainer = null;
        }

        public override void OnHide()
        {
            base.OnHide();
            ReleaseRender();
        }

        protected override void Close()
        {
            ReleaseRender();
            base.Close();
        }
    }
}