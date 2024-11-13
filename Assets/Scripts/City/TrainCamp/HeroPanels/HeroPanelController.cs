using City.SliderCity;
using City.TrainCamp.HeroInstances;
using City.TrainCamp.HeroPanels;
using City.TrainCamp.HeroPanels.HeroDetails;
using ClientServices;
using Cysharp.Threading.Tasks;
using Db.CommonDictionaries;
using Hero;
using Misc.Json;
using Models.Common;
using Models.Heroes.HeroCharacteristics;
using Models.Heroes.PowerUps;
using Network.DataServer;
using Network.DataServer.Messages.HeroPanels;
using System;
using UIController.SkillPanels;
using UiExtensions.Scroll.Interfaces;
using UniRx;
using VContainer;
using VContainer.Unity;
using VContainerUi.Messages;
using VContainerUi.Model;

namespace City.TrainCamp
{
    public class HeroPanelController : UiPanelController<HeroPanelView>, IInitializable
    {
        [Inject] private readonly CommonDictionaries _dictionaries;
        [Inject] private readonly CommonGameData _commonGameData;
        [Inject] private readonly ResourceStorageController _resourceStorageController;
        [Inject] private readonly HeroInstancesController _heroInstancesController;
        [Inject] private readonly HeroDetailsPanelController _heroDetailsPanel;
        [Inject] private readonly IObjectResolver _objectResolver;
        [Inject] private readonly IJsonConverter _jsonConverter;
        [Inject] private readonly SkillPanelController _skillPanelController;

        private CostLevelUpContainer _costLevelObject;

        private ReactiveCommand<SwipeType> _onSwipe = new();
        private GameHero _currentHero;

        public GameHero Hero => _currentHero;
        public IObservable<SwipeType> OnSwipe => _onSwipe;

        public new void Initialize()
        {
            base.Initialize();
            View.CostController.InjectAll(_objectResolver);

            foreach (var cell in View.CellsForItem)
            {
                _objectResolver.Inject(cell);
            }

            foreach (var cell in View.SkillCells)
                cell.OnSelect.Subscribe(OpenSkillDetails).AddTo(Disposables);

            View.EvolitionPanelButton.OnClickAsObservable().Subscribe(_ => OpenEvolutionPanel()).AddTo(Disposables);
            View.ToLeftButton.OnClickAsObservable().Subscribe(_ => _onSwipe.Execute(SwipeType.Left)).AddTo(Disposables);
            View.ToRightButton.OnClickAsObservable().Subscribe(_ => _onSwipe.Execute(SwipeType.Right)).AddTo(Disposables);
            View.LevelUpButton.OnClickAsObservable().Subscribe(_ => LevelUp()).AddTo(Disposables);
            View.OpenHeroDetailsButton.OnClickAsObservable().Subscribe(_ => DetailsOpen()).AddTo(Disposables);
        }

        private void OpenSkillDetails(SkillCell cell)
        {
            _skillPanelController.ShowSkillData(cell.Data);
        }

        protected override void OnLoadGame()
        {
            _costLevelObject = _dictionaries.CostContainers["Heroes"];
        }

        private void DetailsOpen()
        {
            _heroDetailsPanel.SetData(_currentHero);
            MessagesPublisher.OpenWindowPublisher.OpenWindow<HeroDetailsPanelController>(openType: OpenType.Additive);
        }

        public void ShowHero(GameHero hero)
        {
            _currentHero = hero;
            UpdateInfoAboutHero();
            _heroInstancesController.ShowHero(hero);
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
            View.imageHero.sprite = _currentHero.Avatar;
            View.textNameHero.text = _currentHero.Model.General.HeroId;
            UpdateTextAboutHero();
            foreach (var cell in View.CellsForItem)
            {
                cell.Clear();
                cell.SetData(_currentHero.Costume.GetItem(cell.CellType));
            }
            CheckControllers();
            View.RatingHeroController.ShowRating(_currentHero.HeroData.Rating);
        }

        public void UpdateTextAboutHero()
        {
            View.textLevel.text = $"{_currentHero.HeroData.Level}";
            View.textHP.text = ((int)_currentHero.GetCharacteristic(TypeCharacteristic.HP)).ToString();
            View.textAttack.text = ((int)_currentHero.GetCharacteristic(TypeCharacteristic.Damage)).ToString();
            View.textArmor.text = ((int)_currentHero.GetCharacteristic(TypeCharacteristic.Defense)).ToString();
            View.textInitiative.text = ((int)_currentHero.GetCharacteristic(TypeCharacteristic.Initiative)).ToString();
            View.textStrengthHero.text = _currentHero.Strength.ToString();
            //_hero.PrepareSkillLocalization();
            ShowSkills();
            View.CostController.ShowCosts(_costLevelObject.GetCostForLevelUp(_currentHero.HeroData.Level));
            // _heroDetailsPanel.ShowDetails(_hero);
        }

        private void ShowSkills()
        {
            for(var i = 0; i < _currentHero.Model.Skills.Count; i++)
            {
                View.SkillCells[i].SetData(_currentHero.Model.Skills[i]);
                View.SkillCells[i].OffObject();
            }
        }

        private void CheckControllers()
        {
            var nextRaitingContainerId = $"{_currentHero.HeroData.Rating + 1}";
            View.EvolitionPanelButton.gameObject
                .SetActive(_dictionaries.RatingUpContainers.ContainsKey(nextRaitingContainerId));

            var heroLevel = _currentHero.HeroData.Level;
            var heroBreakthrough = _currentHero.HeroData.CurrentBreakthrough;
            var limitLevel = heroLevel;

            if (heroBreakthrough < _currentHero.Model.Evolutions.Stages.Count)
                limitLevel = _currentHero.Model.Evolutions.Stages[heroBreakthrough].NewLimitLevel;

            var enoughResource = _resourceStorageController
                .CheckResource(_costLevelObject.GetCostForLevelUp(_currentHero.HeroData.Level));

            View.LevelUpButton.gameObject.SetActive(heroLevel < limitLevel);
            View.LevelUpButton.interactable = enoughResource;
        }

        public void LevelUp()
        {
            var cost = _costLevelObject.GetCostForLevelUp(_currentHero.HeroData.Level);
            if (_resourceStorageController.CheckResource(cost))
            {
                LevelUpMessage().Forget();
                _resourceStorageController.SubtractResource(cost);
                _currentHero.LevelUp();
                UpdateInfoAboutHero();
                _heroInstancesController.ShowAnimation();
            }
        }

        private async UniTaskVoid LevelUpMessage()
        {
            var message = new HeroLevelUpMessage() { PlayerId = _commonGameData.PlayerInfoData.Id, HeroId = _currentHero.HeroData.Id };
            var result = await DataServer.PostData(message);
            UnityEngine.Debug.Log(result);
        }

        private void OpenEvolutionPanel()
        {
            MessagesPublisher.OpenWindowPublisher.OpenWindow<HeroEvolutionPanelController>(openType: OpenType.Additive);
        }

        protected override void Close()
        {
            _heroInstancesController.Hide();
            base.Close();
        }
    }
}