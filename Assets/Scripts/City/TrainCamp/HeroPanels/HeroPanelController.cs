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
using Network.DataServer;
using Network.DataServer.Messages.HeroPanels;
using System;
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

        private CostLevelUpContainer _costLevelObject;

        private ReactiveCommand<SwipeType> _onSwipe = new();
        private GameHero _hero;

        public GameHero Hero => _hero;
        public IObservable<SwipeType> OnSwipe => _onSwipe;

        public new void Initialize()
        {
            base.Initialize();
            View.CostController.InjectAll(_objectResolver);

            foreach (var cell in View.CellsForItem)
            {
                _objectResolver.Inject(cell);
            }

            View.EvolitionPanelButton.OnClickAsObservable().Subscribe(_ => OpenEvolutionPanel()).AddTo(Disposables);
            View.ToLeftButton.OnClickAsObservable().Subscribe(_ => _onSwipe.Execute(SwipeType.Left)).AddTo(Disposables);
            View.ToRightButton.OnClickAsObservable().Subscribe(_ => _onSwipe.Execute(SwipeType.Right)).AddTo(Disposables);
            View.LevelUpButton.OnClickAsObservable().Subscribe(_ => LevelUp()).AddTo(Disposables);
            View.OpenHeroDetailsButton.OnClickAsObservable().Subscribe(_ => DetailsOpen()).AddTo(Disposables);
        }

        protected override void OnLoadGame()
        {
            _costLevelObject = _dictionaries.CostContainers["Heroes"];
        }

        private void DetailsOpen()
        {
            _heroDetailsPanel.SetData(_hero);
            MessagesPublisher.OpenWindowPublisher.OpenWindow<HeroDetailsPanelController>(openType: OpenType.Additive);
        }

        public void ShowHero(GameHero hero)
        {
            _hero = hero;
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
            View.imageHero.sprite = _hero.Avatar;
            View.textNameHero.text = _hero.Model.General.HeroId;
            UpdateTextAboutHero();
            foreach (var cell in View.CellsForItem)
            {
                cell.Clear();
                cell.SetData(_hero.Costume.GetItem(cell.CellType));
            }
            CheckResourceForLevelUP();
            View.RatingHeroController.ShowRating(_hero.HeroData.Rating);
        }

        public void UpdateTextAboutHero()
        {
            View.textLevel.text = $"{_hero.HeroData.Level}";
            View.textHP.text = ((int)_hero.GetCharacteristic(TypeCharacteristic.HP)).ToString();
            View.textAttack.text = ((int)_hero.GetCharacteristic(TypeCharacteristic.Damage)).ToString();
            View.textArmor.text = ((int)_hero.GetCharacteristic(TypeCharacteristic.Defense)).ToString();
            View.textInitiative.text = ((int)_hero.GetCharacteristic(TypeCharacteristic.Initiative)).ToString();
            View.textStrengthHero.text = _hero.Strength.ToString();
            //_hero.PrepareSkillLocalization();
            View.skillController.ShowSkills(_hero.Model.Skills);
            View.CostController.ShowCosts(_costLevelObject.GetCostForLevelUp(_hero.HeroData.Level));
            // _heroDetailsPanel.ShowDetails(_hero);
        }

        private void CheckResourceForLevelUP()
        {
            View.LevelUpButton.interactable = _resourceStorageController.CheckResource(_costLevelObject.GetCostForLevelUp(_hero.HeroData.Level));
        }

        public void LevelUp()
        {
            var cost = _costLevelObject.GetCostForLevelUp(_hero.HeroData.Level);
            if (_resourceStorageController.CheckResource(cost))
            {
                LevelUpMessage().Forget();
                _resourceStorageController.SubtractResource(cost);
                _hero.LevelUp();
                UpdateInfoAboutHero();
                _heroInstancesController.ShowAnimation();
            }
        }

        private async UniTaskVoid LevelUpMessage()
        {
            var message = new HeroLevelUpMessage() { PlayerId = _commonGameData.PlayerInfoData.Id, HeroId = _hero.HeroData.Id };
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