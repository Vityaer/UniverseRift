using Assets.Scripts.ClientServices;
using City.SliderCity;
using City.TrainCamp.HeroPanels;
using City.TrainCamp.HeroPanels.HeroDetails;
using Db.CommonDictionaries;
using Hero;
using Models.Heroes.HeroCharacteristics;
using System;
using System.Linq;
using UIController.Inventory;
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
        [Inject] private readonly ResourceStorageController _resourceStorageController;
        [Inject] private readonly HeroDetailsPanelController _heroDetailsPanel;
        [Inject] private IObjectResolver _objectResolver;

        private CostLevelUpContainer costLevelObject;

        private ReactiveCommand<SwipeType> _onSwipe = new ReactiveCommand<SwipeType>();
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
            costLevelObject = _dictionaries.CostContainers["Heroes"];
        }

        private void DetailsOpen()
        {
            _heroDetailsPanel.SetData(_hero);
            _messagesPublisher.OpenWindowPublisher.OpenWindow<HeroDetailsPanelController>(openType: OpenType.Additive);
        }

        public void ShowHero(GameHero hero)
        {
            _hero = hero;
            UpdateInfoAboutHero();
        }

        public void UpdateInfoAboutHero()
        {
            View.imageHero.sprite = _hero.Avatar;
            View.textNameHero.text = _hero.Model.General.Name;
            UpdateTextAboutHero();
            foreach (var cell in View.CellsForItem)
            {
                //cell.Clear();
                //cell.SetItem(_hero.Costume.Items[cell.CellType]);
            }
            CheckResourceForLevelUP();
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
            View.CostController.ShowCosts(costLevelObject.GetCostForLevelUp(_hero.HeroData.Level));
            // _heroDetailsPanel.ShowDetails(_hero);
        }

        private void CheckResourceForLevelUP()
        {
            View.LevelUpButton.interactable = _resourceStorageController.CheckResource(costLevelObject.GetCostForLevelUp(_hero.HeroData.Level));
        }

        public void TakeOff(GameItem item)
        {
            _hero.Costume.TakeOff(item);
            UpdateTextAboutHero();
        }

        public void LevelUp()
        {
            var cost = costLevelObject.GetCostForLevelUp(_hero.HeroData.Level);
            if (_resourceStorageController.CheckResource(cost))
            {
                _resourceStorageController.SubtractResource(cost);
                _hero.LevelUp();
                UpdateInfoAboutHero();
            }
        }

        private void OpenEvolutionPanel()
        {
            _messagesPublisher.OpenWindowPublisher.OpenWindow<HeroEvolutionPanelController>(openType: OpenType.Additive);
        }
    }
}