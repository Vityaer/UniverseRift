using City.Buildings.CityButtons;
using City.Buildings.CityButtons.EventAgent;
using City.Buildings.PlayerPanels;
using City.Panels.DailyRewards;
using City.Panels.DailyTasks;
using UnityEngine;
using VContainer;
using VContainerUi;
using VContainer.Extensions;
using City.Panels.Registrations;
using City.TrainCamp;
using City.Panels.RatingUps;
using City.Panels.PosibleHeroes;
using City.Panels.SubjectPanels;
using City.Panels.SubjectPanels.Resources;
using UIController.Inventory;
using City.Panels.SubjectPanels.Splinters;
using City.Panels.NewLevels;
using City.Panels.Inventories;
using City.TrainCamp.HeroPanels;
using City.TrainCamp.HeroPanels.HeroDetails;
using City.Panels.BoxRewards;
using City.Buildings.Requirement;
using UIController;
using UIController.ControllerPanels.MarketResources;
using UIController.ControllerPanels.PlayerNames;
using UIController.ControllerPanels.SelectCount;
using City.Panels.AutoFights;
using City.Buildings.TravelCircle.PanelMissions;
using City.General;
using City.Buildings.Friends;
using City.Buildings.Mails;
using City.Panels.SelectHeroes;
using Campaign.GoldHeaps;
using Campaign;

namespace Installers
{
    [CreateAssetMenu(menuName = "UI/Installer/" + nameof(PanelsUiInstaller), fileName = nameof(PanelsUiInstaller), order = 0)]
    public class PanelsUiInstaller : ScriptableObjectInstaller
    {
        private const string MAIN_PANELS = "Panels";
        private const int CANVAS_ORDER = 2;

        [SerializeField] private Canvas _canvas;
        [SerializeField] private DailyRewardPanelView _dailyRewardPanelView;
        [SerializeField] private DailyTaskPanelView _dailyTaskPanelView;
        [SerializeField] private PlayerPanelView _playerPanelView;
        [SerializeField] private RegistrationPanelView _registrationPanelView;
        [SerializeField] private HeroEvolutionPanelView _ratingUpPanelView;
        [SerializeField] private PosibleHeroesPanelView _posibleHeroesPanelView;
        [SerializeField] private ItemPanelView _itemPanelView;
        [SerializeField] private ResourcePanelView _resourcePanelView;
        [SerializeField] private SplinterPanelView _splinterPanelView;
        [SerializeField] private PlayerNewLevelPanelView _playerNewLevelPanelView;
        [SerializeField] private InventoryView _inventoryView;
        [SerializeField] private HeroPanelView _heroPanelView;
        [SerializeField] private HeroDetailsPanelView _heroDetailsPanelView;
        [SerializeField] private BoxRewardsPanelView _boxRewardView;
        [SerializeField] private AchievmentsPanelView _achievmentsPanelView;
        [SerializeField] private BuyResourcePanelView _buyResourcePanelView;
        [SerializeField] private PlayerNewNamePanelView _playerNewNamePanelView;
        [SerializeField] private SplinterSelectCountPanelView _splinterSelectCountPanelView;
        [SerializeField] private AutoFightRewardPanelView _autoFightRewardPanelView;
        [SerializeField] private TravelMissionsPanelView _travelMissionsPanelView;
        [SerializeField] private FriendsView _friendsView;
        [SerializeField] private MailView _mailView;
        [SerializeField] private GoldHeapView _goldHeapView;

        public override void Install(IContainerBuilder builder)
        {
            var canvas = Instantiate(_canvas);
            canvas.gameObject.name = MAIN_PANELS;
            canvas.GetComponent<Canvas>().sortingOrder = CANVAS_ORDER;

            builder.RegisterUiView<DailyRewardPanelController, DailyRewardPanelView>(_dailyRewardPanelView, canvas.transform);
            builder.RegisterUiView<DailyTaskPanelController, DailyTaskPanelView>(_dailyTaskPanelView, canvas.transform);
            builder.RegisterUiView<PlayerPanelController, PlayerPanelView>(_playerPanelView, canvas.transform);
            builder.RegisterUiView<RegisterationPanelController, RegistrationPanelView>(_registrationPanelView, canvas.transform);
            builder.RegisterUiView<HeroEvolutionPanelController, HeroEvolutionPanelView>(_ratingUpPanelView, canvas.transform);
            builder.RegisterUiView<PosibleHeroesPanelController, PosibleHeroesPanelView>(_posibleHeroesPanelView, canvas.transform);
            builder.RegisterUiView<ItemPanelController, ItemPanelView>(_itemPanelView, canvas.transform);
            builder.RegisterUiView<ResourcePanelController, ResourcePanelView>(_resourcePanelView, canvas.transform);
            builder.RegisterUiView<SplinterPanelController, SplinterPanelView>(_splinterPanelView, canvas.transform);
            builder.RegisterUiView<PlayerNewLevelPanelController, PlayerNewLevelPanelView>(_playerNewLevelPanelView, canvas.transform);
            builder.RegisterUiView<InventoryController, InventoryView>(_inventoryView, canvas.transform);
            builder.RegisterUiView<HeroPanelController, HeroPanelView>(_heroPanelView, canvas.transform);
            builder.RegisterUiView<HeroDetailsPanelController, HeroDetailsPanelView>(_heroDetailsPanelView, canvas.transform);
            builder.RegisterUiView<BoxRewardsPanelController, BoxRewardsPanelView>(_boxRewardView, canvas.transform);
            builder.RegisterUiView<AchievmentsPageController, AchievmentsPanelView>(_achievmentsPanelView, canvas.transform);
            builder.RegisterUiView<BuyResourcePanelController, BuyResourcePanelView>(_buyResourcePanelView, canvas.transform);
            builder.RegisterUiView<PlayerNewNamePanelController, PlayerNewNamePanelView>(_playerNewNamePanelView, canvas.transform);
            builder.RegisterUiView<SplinterSelectCountPanelController, SplinterSelectCountPanelView>(_splinterSelectCountPanelView, canvas.transform);
            builder.RegisterUiView<AutoFightRewardPanelController, AutoFightRewardPanelView>(_autoFightRewardPanelView, canvas.transform);
            builder.RegisterUiView<TravelMissionsPanelController, TravelMissionsPanelView>(_travelMissionsPanelView, canvas.transform);
            builder.RegisterUiView<FriendsController, FriendsView>(_friendsView, canvas.transform);
            builder.RegisterUiView<MailController, MailView>(_mailView, canvas.transform);
            builder.RegisterUiView<GoldHeapController, GoldHeapView>(_goldHeapView, canvas.transform);
        }
    }
}
