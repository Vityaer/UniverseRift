using Assets.Scripts.City.Panels.Arenas;
using Buildings.Mails.LetterPanels;
using Campaign;
using Campaign.GoldHeaps;
using City.Buildings.CityButtons;
using City.Buildings.CityButtons.EventAgent;
using City.Buildings.Friends;
using City.Buildings.Friends.Panels.AvailableFriends;
using City.Buildings.Friends.Panels.FriendRequests;
using City.Buildings.Guild.AvailableGuildsPanels;
using City.Buildings.Guild.BossRaid;
using City.Buildings.Guild.BossRaidRewardPanels;
using City.Buildings.Guild.GuildAdministrations;
using City.Buildings.Guild.GuildDonatePanels;
using City.Buildings.Guild.GuildMarket;
using City.Buildings.Guild.GuildRecruitDetailPanels;
using City.Buildings.Guild.GuildTaskboardPanels;
using City.Buildings.Guild.NewGuildPanels;
using City.Buildings.Guild.RecruitRequestPanels;
using City.Buildings.Mails;
using City.Buildings.Mails.LetterPanels;
using City.Buildings.Mines;
using City.Buildings.Mines.Panels;
using City.Buildings.Mines.Panels.CreateMines;
using City.Buildings.Mines.Panels.Travels;
using City.Buildings.PlayerPanels;
using City.Buildings.PlayerPanels.AvatarPanels;
using City.Buildings.PlayerPanels.AvatarPanels.AvatarPanelDetails;
using City.Buildings.Requirement;
using City.Buildings.Voyage;
using City.Buildings.Voyage.Panels;
using City.Panels.Achievments;
using City.Panels.Arenas;
using City.Panels.Arenas.RatingArenas;
using City.Panels.Arenas.SimpleArenas;
using City.Panels.Arenas.Tournaments;
using City.Panels.AutoFights;
using City.Panels.BatllepasPanels;
using City.Panels.BoxRewards;
using City.Panels.Chats.ServerChats;
using City.Panels.Confirmations;
using City.Panels.CreatorMessagePanels;
using City.Panels.DailyRewards;
using City.Panels.DailyTasks;
using City.Panels.Events.FortuneCycles;
using City.Panels.Events.HeroSpaceMarkets;
using City.Panels.Events.RaceCircleCycles;
using City.Panels.Events.SweetCycles;
using City.Panels.Events.TavernCycleMainPanels;
using City.Panels.Helps;
using City.Panels.HeroesHireResultPanels;
using City.Panels.Inventories;
using City.Panels.Misc.Settings;
using City.Panels.MonthTasks.Abstractions;
using City.Panels.MonthTasks.Arena;
using City.Panels.MonthTasks.Evolutions;
using City.Panels.MonthTasks.Taskboards;
using City.Panels.MonthTasks.Travels;
using City.Panels.NewLevels;
using City.Panels.OtherPlayers.MainPanels;
using City.Panels.PlayerInfoPanels;
using City.Panels.PosibleHeroes;
using City.Panels.RatingUps;
using City.Panels.RatingUps.EvolutionResultPanels;
using City.Panels.Registrations;
using City.Panels.Rewards;
using City.Panels.ScreenBlockers;
using City.Panels.SubjectPanels;
using City.Panels.SubjectPanels.Common;
using City.Panels.SubjectPanels.Items;
using City.Panels.SubjectPanels.Resources;
using City.Panels.SubjectPanels.Splinters;
using City.Panels.WhereGetSubjectPanels;
using City.TrainCamp;
using City.TrainCamp.HeroPanels;
using City.TrainCamp.HeroPanels.HeroDetails;
using MainPages.Events.Cycles.TavernCycles.Panels;
using Ui.LoadingScreen.Loading;
using Ui.LoadingScreen.ProgressBar;
using UIController;
using UIController.Common;
using UIController.ControllerPanels.AlchemyPanels;
using UIController.ControllerPanels.MarketResources;
using UIController.ControllerPanels.PlayerNames;
using UIController.ControllerPanels.SelectCount;
using UIController.FadeInOutPanels;
using UIController.Inventory;
using UIController.LoadingUI;
using UIController.SkillPanels;
using UnityEngine;
using UnityEngine.Serialization;
using VContainer;
using VContainer.Extensions;
using VContainerUi;

namespace Installers
{
    [CreateAssetMenu(menuName = "UI/Installer/" + nameof(PanelsUiInstaller), fileName = nameof(PanelsUiInstaller), order = 0)]
    public class PanelsUiInstaller : ScriptableObjectInstaller
    {
        private const string MAIN_PANELS = "Panels";
        private const int CANVAS_ORDER = 100;

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
        [SerializeField] private HeroPanelView _heroPanelView;
        [SerializeField] private HeroDetailsPanelView _heroDetailsPanelView;
        [SerializeField] private BoxRewardsPanelView _boxRewardView;
        [SerializeField] private AchievmentsPanelView _achievmentsPanelView;
        [SerializeField] private BuyResourcePanelView _buyResourcePanelView;
        [SerializeField] private PlayerNewNamePanelView _playerNewNamePanelView;
        [SerializeField] private SplinterSelectCountPanelView _splinterSelectCountPanelView;
        [SerializeField] private AutoFightRewardPanelView _autoFightRewardPanelView;
        [SerializeField] private FriendsPanelView _friendsView;
        [SerializeField] private GoldHeapView _goldHeapView;
        [SerializeField] private InventoryView _inventoryView;
        [SerializeField] private RewardPanelView _rewardPanelView;
        [SerializeField] private RatingArenaPanelView _ratingArenaPanelView;
        [SerializeField] private SimpleArenaPanelView _simpleArenaPanelView;
        [SerializeField] private TournamentPanelView _tournamentPanelView;
        [SerializeField] private InfoMinePanelView _infoMinePanelView;
        [SerializeField] private CreateMinePanelView _createMinePanelView;
        [SerializeField] private BattlepasPanelView _battlepasPanelView;
        [SerializeField] private FriendRequestsPanelView _friendRequestsPanelView;
        [SerializeField] private AvailableFriendsPanelView _availableFriendsPanelView;
        [SerializeField] private OtherPlayerMainPanelView _otherPlayerMainPanelView;
        [SerializeField] private ConfirmationPanelView _confirmationPanelView;
        [SerializeField] private VoyageMissionPanelView _voyageMissionPanelView;
        [SerializeField] private AlchemyPanelView _alchemyPanelView;
        [SerializeField] private EvolutionResultPanelView _evolutionResultPanelView;

        [SerializeField] private SkillPanelView _skillPanelView;

        [Header("Mails")]
        [SerializeField] private MailPanelView _mailView;
        [SerializeField] private CreatorMessagePanelView _creatorMessagePanelView;
        [SerializeField] private LetterPanelView _letterPanelView;
        [SerializeField] private ServerChatPanelView _serverChatPanelView;

        [Header("Month Tasks")]
        [SerializeField] private MonthArenaPanelView _monthArenaPanelView;
        [SerializeField] private MonthEvolutionPanelView _monthEvolutionPanelView;
        [SerializeField] private MonthTaskboardPanelView _monthTaskboardPanelView;
        [SerializeField] private MonthTravelPanelView _monthTravelPanelView;


        [Header("Guild")]
        [SerializeField] private GuildDonatePanelView _guildDonatePanelView;
        [SerializeField] private GuildTaskboardPanelView _guildTaskboardPanelView;
        [SerializeField] private NewGuildPanelView _newGuildPanelView;
        [SerializeField] private AvailableGuildsPanelView _availableGuildsPanelView;
        [SerializeField] private GuildBossRaidPanelView _guildBossRaidPanelController;
        [SerializeField] private GuildAdministrationPanelView _guildAdministrationPanelView;
        [SerializeField] private RecruitRequestPanelView _recruitRequestPanelView;
        [SerializeField] private GuildMarketView _guildMarketView;
        [SerializeField] private GuildRecruitDetailPanelView _guildRecruitDetailPanelView;
        [SerializeField] private BossRaidRewardPanelView _bossRaidRewardPanelView;
        
        [Header("General")]
        
        [SerializeField] private AvatarPanelDetailsView _avatarPanelDetailsView;
        [SerializeField] private AvatarPanelView _avatarPanelView;

        [Header("Events")]
        [SerializeField] private TavernCycleMainPanelView _tavernCycleMainPanelView;
        [SerializeField] private FortuneCycleMainPanelView _fortuneCycleMainPanelView;
        [SerializeField] private SweetCycleMainPanelView _sweetCycleMainPanelView;
        [SerializeField] private RaceCicrleCycleMainPanelView _raceCicrleCycleMainPanelView;

        [Header("Heroes")]
        [SerializeField] private HeroesHireResultPanelView _heroesHireResultPanelView;

        [Header("Mines")]
        [SerializeField] private MineTravelPanelView _mineTravelPanelView;

        [Header("FideInOut")]
        [SerializeField] private FadeInOutPanelView _fadeInOutPanelView;

        [Header("Misc")]
        [SerializeField] private StartLoadingView _startLoadingView;
        [SerializeField] private LoadingView _loadingView;
        [SerializeField] private ProgressBarView _progressBarView;
        [SerializeField] private PlayerMiniInfoPanelView _playerMiniInfoPanelView;
        [SerializeField] private SettingsPanelView _settingsPanelView;
        [SerializeField] private HelpPanelView _helpPanelView;
        [SerializeField] private ScreenBlockerView _ScreenBlockerView;
        [SerializeField] private HeroMarketView _heroMarketView;
        [SerializeField] private WhereGetSubjectPanelView _whereGetSubjectPanelView;
        
        public override void Install(IContainerBuilder builder)
        {
            var canvas = Instantiate(_canvas);
            canvas.gameObject.name = MAIN_PANELS;
            canvas.GetComponent<Canvas>().sortingOrder = CANVAS_ORDER;

            builder.RegisterUiView<DailyRewardPanelController, DailyRewardPanelView>(_dailyRewardPanelView, canvas.transform);
            builder.RegisterUiView<DailyTaskPanelController, DailyTaskPanelView>(_dailyTaskPanelView, canvas.transform);
            builder.RegisterUiView<PlayerPanelController, PlayerPanelView>(_playerPanelView, canvas.transform);
            builder.RegisterUiView<RegistrationPanelController, RegistrationPanelView>(_registrationPanelView, canvas.transform);
            builder.RegisterUiView<HeroEvolutionPanelController, HeroEvolutionPanelView>(_ratingUpPanelView, canvas.transform);
            builder.RegisterUiView<EvolutionResultPanelController, EvolutionResultPanelView>(_evolutionResultPanelView, canvas.transform);
            builder.RegisterUiView<PosibleHeroesPanelController, PosibleHeroesPanelView>(_posibleHeroesPanelView, canvas.transform);
            builder.RegisterUiView<ItemPanelController, ItemPanelView>(_itemPanelView, canvas.transform);
            builder.RegisterUiView<ResourcePanelController, ResourcePanelView>(_resourcePanelView, canvas.transform);
            builder.RegisterUiView<SplinterPanelController, SplinterPanelView>(_splinterPanelView, canvas.transform);
            builder.RegisterUiView<PlayerNewLevelPanelController, PlayerNewLevelPanelView>(_playerNewLevelPanelView, canvas.transform);
            builder.RegisterUiView<HeroPanelController, HeroPanelView>(_heroPanelView, canvas.transform);
            builder.RegisterUiView<SkillPanelController, SkillPanelView>(_skillPanelView, canvas.transform);
            builder.RegisterUiView<HeroDetailsPanelController, HeroDetailsPanelView>(_heroDetailsPanelView, canvas.transform);
            builder.RegisterUiView<BoxRewardsPanelController, BoxRewardsPanelView>(_boxRewardView, canvas.transform);
            builder.RegisterUiView<AchievmentsPageController, AchievmentsPanelView>(_achievmentsPanelView, canvas.transform);
            builder.RegisterUiView<BuyResourcePanelController, BuyResourcePanelView>(_buyResourcePanelView, canvas.transform);
            builder.RegisterUiView<PlayerNewNamePanelController, PlayerNewNamePanelView>(_playerNewNamePanelView, canvas.transform);
            builder.RegisterUiView<SplinterSelectCountPanelController, SplinterSelectCountPanelView>(_splinterSelectCountPanelView, canvas.transform);
            builder.RegisterUiView<AutoFightRewardPanelController, AutoFightRewardPanelView>(_autoFightRewardPanelView, canvas.transform);
            builder.RegisterUiView<FriendsPanelController, FriendsPanelView>(_friendsView, canvas.transform);
            builder.RegisterUiView<GoldHeapController, GoldHeapView>(_goldHeapView, canvas.transform);
            builder.RegisterUiView<InventoryPanelController, InventoryView>(_inventoryView, canvas.transform);
            builder.RegisterUiView<RewardPanelController, RewardPanelView>(_rewardPanelView, canvas.transform);
            
            builder.RegisterUiView<InfoMinePanelController, InfoMinePanelView>(_infoMinePanelView, canvas.transform);
            builder.RegisterUiView<CreateMinePanelController, CreateMinePanelView>(_createMinePanelView, canvas.transform);
            builder.RegisterUiView<MineTravelPanelController, MineTravelPanelView>(_mineTravelPanelView, canvas.transform);

            builder.RegisterUiView<RatingArenaPanelController, RatingArenaPanelView>(_ratingArenaPanelView, canvas.transform);
            builder.RegisterUiView<SimpleArenaPanelController, SimpleArenaPanelView>(_simpleArenaPanelView, canvas.transform);
            builder.RegisterUiView<TournamentPanelController, TournamentPanelView>(_tournamentPanelView, canvas.transform);

            builder.RegisterUiView<NewGuildPanelController, NewGuildPanelView>(_newGuildPanelView, canvas.transform);
            builder.RegisterUiView<AvailableGuildsPanelController, AvailableGuildsPanelView>(_availableGuildsPanelView, canvas.transform);
            builder.RegisterUiView<GuildBossRaidPanelController, GuildBossRaidPanelView>(_guildBossRaidPanelController, canvas.transform);
            builder.RegisterUiView<GuildAdministrationPanelController, GuildAdministrationPanelView>(_guildAdministrationPanelView, canvas.transform);
            builder.RegisterUiView<GuildDonatePanelController, GuildDonatePanelView>(_guildDonatePanelView, canvas.transform);
            builder.RegisterUiView<GuildTaskboardPanelController, GuildTaskboardPanelView>(_guildTaskboardPanelView, canvas.transform);
            builder.RegisterUiView<RecruitRequestPanelController, RecruitRequestPanelView>(_recruitRequestPanelView, canvas.transform);
            builder.RegisterUiView<GuildMarketController, GuildMarketView>(_guildMarketView, canvas.transform);
            builder.RegisterUiView<GuildRecruitDetailPanelController, GuildRecruitDetailPanelView>(_guildRecruitDetailPanelView, canvas.transform);
            builder.RegisterUiView<BossRaidRewardPanelController, BossRaidRewardPanelView>(_bossRaidRewardPanelView, canvas.transform);
            
            builder.RegisterUiView<BattlepasPanelController, BattlepasPanelView>(_battlepasPanelView, canvas.transform);
            
            builder.RegisterUiView<FriendRequestsPanelController, FriendRequestsPanelView>(_friendRequestsPanelView, canvas.transform);
            builder.RegisterUiView<AvailableFriendsPanelController, AvailableFriendsPanelView>(_availableFriendsPanelView, canvas.transform);
            builder.RegisterUiView<OtherPlayerMainPanelController, OtherPlayerMainPanelView>(_otherPlayerMainPanelView, canvas.transform);
            builder.RegisterUiView<PlayerMiniInfoPanelController, PlayerMiniInfoPanelView>(_playerMiniInfoPanelView, canvas.transform);

            builder.RegisterUiView<ServerChatPanelController, ServerChatPanelView>(_serverChatPanelView, canvas.transform);
            builder.RegisterUiView<MailPanelController, MailPanelView>(_mailView, canvas.transform);
            builder.RegisterUiView<CreatorMessagePanelController, CreatorMessagePanelView>(_creatorMessagePanelView, canvas.transform);
            builder.RegisterUiView<LetterPanelController, LetterPanelView>(_letterPanelView, canvas.transform);

            builder.RegisterUiView<ConfirmationPanelController, ConfirmationPanelView>(_confirmationPanelView, canvas.transform);
        
            builder.RegisterUiView<MonthArenaPanelController, MonthArenaPanelView>(_monthArenaPanelView, canvas.transform);
            builder.RegisterUiView<MonthEvolutionPanelController, MonthEvolutionPanelView>(_monthEvolutionPanelView, canvas.transform);
            builder.RegisterUiView<MonthTaskboardPanelController, MonthTaskboardPanelView>(_monthTaskboardPanelView, canvas.transform);
            builder.RegisterUiView<MonthTravelPanelController, MonthTravelPanelView>(_monthTravelPanelView, canvas.transform);

            builder.RegisterUiView<VoyageMissionPanelController, VoyageMissionPanelView>(_voyageMissionPanelView, canvas.transform);

            builder.RegisterUiView<AvatarPanelController, AvatarPanelView>(_avatarPanelView, canvas.transform);
            builder.RegisterUiView<AvatarPanelDetailsController, AvatarPanelDetailsView>(_avatarPanelDetailsView, canvas.transform);
            
            builder.RegisterUiView<TavernCycleMainPanelController, TavernCycleMainPanelView>(_tavernCycleMainPanelView, canvas.transform);
            builder.RegisterUiView<FortuneCycleMainPanelController, FortuneCycleMainPanelView>(_fortuneCycleMainPanelView, canvas.transform);
            builder.RegisterUiView<SweetCycleMainPanelController, SweetCycleMainPanelView>(_sweetCycleMainPanelView, canvas.transform);
            builder.RegisterUiView<RaceCicrleCycleMainPanelController, RaceCicrleCycleMainPanelView>(_raceCicrleCycleMainPanelView, canvas.transform);
           
            builder.RegisterUiView<HeroMarketController, HeroMarketView>(_heroMarketView, canvas.transform);
            
            builder.RegisterUiView<AlchemyPanelController, AlchemyPanelView>(_alchemyPanelView, canvas.transform);

            builder.RegisterUiView<HeroesHireResultPanelController, HeroesHireResultPanelView>(_heroesHireResultPanelView, canvas.transform);

            builder.RegisterUiView<StartLoadingController, StartLoadingView>(_startLoadingView, canvas.transform);
            builder.RegisterUiView<LoadingController, LoadingView>(_loadingView, canvas.transform);
            builder.RegisterUiView<ProgressBarController, ProgressBarView>(_progressBarView, canvas.transform);
            builder.RegisterUiView<FadeInOutPanelController, FadeInOutPanelView>(_fadeInOutPanelView, canvas.transform);
            builder.RegisterUiView<SettingsPanelController, SettingsPanelView>(_settingsPanelView, canvas.transform);

            builder.RegisterUiView<HelpPanelController, HelpPanelView>(_helpPanelView, canvas.transform);

            builder.Register<SubjectDetailController>(Lifetime.Singleton).AsImplementedInterfaces().AsSelf();
            builder.RegisterUiView<WhereGetSubjectPanelController, WhereGetSubjectPanelView>(_whereGetSubjectPanelView, canvas.transform);
            
            builder.RegisterUiView<ScreenBlockerController, ScreenBlockerView>(_ScreenBlockerView, canvas.transform, false);
        }
    }
}
