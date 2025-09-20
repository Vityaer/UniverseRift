using System;
using City.SliderCity;
using City.TrainCamp.HeroInstances;
using City.TrainCamp.HeroPanels.HeroDetails;
using ClientServices;
using Common.Db.CommonDictionaries;
using Cysharp.Threading.Tasks;
using Hero;
using LocalizationSystems;
using Misc.Json;
using Models.Common;
using Models.Heroes.HeroCharacteristics;
using Network.DataServer;
using Network.DataServer.Messages.HeroPanels;
using UIController.SkillPanels;
using UiExtensions.Scroll.Interfaces;
using UniRx;
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

        private CostLevelUpContainer m_costLevelObject;

        private readonly ReactiveCommand<SwipeType> m_onSwipe = new();
        private GameHero m_currentHero;

        public GameHero Hero => m_currentHero;
        public IObservable<SwipeType> OnSwipe => m_onSwipe;

        public new void Initialize()
        {
            base.Initialize();
            View.CostController.InjectAll(m_objectResolver);

            foreach (var cell in View.CellsForItem)
            {
                m_objectResolver.Inject(cell);
            }

            foreach (var cell in View.SkillCells)
                cell.OnSelect.Subscribe(OpenSkillDetails).AddTo(Disposables);

            View.EvolitionPanelButton.OnClickAsObservable().Subscribe(_ => OpenEvolutionPanel()).AddTo(Disposables);
            View.ToLeftButton.OnClickAsObservable().Subscribe(_ => m_onSwipe.Execute(SwipeType.Left)).AddTo(Disposables);
            View.ToRightButton.OnClickAsObservable().Subscribe(_ => m_onSwipe.Execute(SwipeType.Right)).AddTo(Disposables);
            View.LevelUpButton.OnClickAsObservable().Subscribe(_ => LevelUp()).AddTo(Disposables);
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
            m_heroInstancesController.ShowHero(hero);
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
            foreach (var cell in View.CellsForItem)
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
            View.textHP.text = ((int)m_currentHero.GetCharacteristic(TypeCharacteristic.HP)).ToString();
            View.textAttack.text = ((int)m_currentHero.GetCharacteristic(TypeCharacteristic.Damage)).ToString();
            View.textArmor.text = ((int)m_currentHero.GetCharacteristic(TypeCharacteristic.Defense)).ToString();
            View.textInitiative.text = ((int)m_currentHero.GetCharacteristic(TypeCharacteristic.Initiative)).ToString();
            View.textStrengthHero.text = m_currentHero.Strength.ToString();
            ShowSkills();
            View.CostController.ShowCosts(m_costLevelObject.GetCostForLevelUp(m_currentHero.HeroData.Level));
        }

        private void ShowSkills()
        {
            for(var i = 0; i < m_currentHero.Model.Skills.Count && i < View.SkillCells.Count; i++)
            {
                View.SkillCells[i].SetData(m_currentHero.Model.Skills[i]);
            }

            for (var i = m_currentHero.Model.Skills.Count; i < View.SkillCells.Count; i++)
            {
                View.SkillCells[i].OffObject();
            }
        }

        private void CheckControllers()
        {
            var nextRaitingContainerId = $"{m_currentHero.HeroData.Rating + 1}";
            View.EvolitionPanelButton.gameObject
                .SetActive(m_dictionaries.RatingUpContainers.ContainsKey(nextRaitingContainerId));

            var heroLevel = m_currentHero.HeroData.Level;
            var heroBreakthrough = m_currentHero.HeroData.CurrentBreakthrough;
            var limitLevel = heroLevel;

            if (heroBreakthrough < m_currentHero.Model.Evolutions.Stages.Count)
                limitLevel = m_currentHero.Model.Evolutions.Stages[heroBreakthrough].NewLimitLevel;

            var enoughResource = m_resourceStorageController
                .CheckResource(m_costLevelObject.GetCostForLevelUp(m_currentHero.HeroData.Level));

            View.LevelUpButton.gameObject.SetActive(heroLevel < limitLevel);
            View.LevelUpButton.interactable = enoughResource;
        }

        public void LevelUp()
        {
            var cost = m_costLevelObject.GetCostForLevelUp(m_currentHero.HeroData.Level);
            if (m_resourceStorageController.CheckResource(cost))
            {
                LevelUpMessage().Forget();
                m_resourceStorageController.SubtractResource(cost);
                m_currentHero.LevelUp();
                UpdateInfoAboutHero();
                m_heroInstancesController.ShowAnimation();
            }
        }

        private async UniTaskVoid LevelUpMessage()
        {
            var message = new HeroLevelUpMessage() { PlayerId = m_commonGameData.PlayerInfoData.Id, HeroId = m_currentHero.HeroData.Id };
            var result = await DataServer.PostData(message);
            UnityEngine.Debug.Log(result);
        }

        private void OpenEvolutionPanel()
        {
            MessagesPublisher.OpenWindowPublisher.OpenWindow<HeroEvolutionPanelController>(openType: OpenType.Additive);
        }

        protected override void Close()
        {
            m_heroInstancesController.Hide();
            base.Close();
        }
    }
}