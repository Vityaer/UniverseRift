using Altar;
using City.Buildings.Altar;
using City.Buildings.Arena;
using City.Buildings.Forge;
using City.Buildings.Friends;
using City.Buildings.Guild;
using City.Buildings.LongTravels;
using City.Buildings.MagicCircle;
using City.Buildings.Mails;
using City.Buildings.Market;
using City.Buildings.Mines;
using City.Buildings.Pets;
using City.Buildings.Sanctuary;
using City.Buildings.TaskGiver;
using City.Buildings.Tavern;
using City.Buildings.Tower;
using City.Buildings.TravelCircle;
using City.Buildings.Voyage;
using City.Buildings.WheelFortune;
using MainPages.City;
using UIController.Common;
using UnityEngine;
using VContainer;
using VContainer.Extensions;
using VContainerUi;

namespace Installers
{
    [CreateAssetMenu(menuName = "UI/Installer/" + nameof(BuildingsInstaller), fileName = nameof(BuildingsInstaller), order = 0)]
    public class BuildingsInstaller : ScriptableObjectInstaller
    {
        private const string MAIN_PANELS = "Buildings";
        private const int CANVAS_ORDER = 1;

        [SerializeField] private Canvas _canvas;
        [SerializeField] private SafeArea _safeArea;
        [SerializeField] private TavernView _tavernView;
        [SerializeField] private MarketView _marketView;
        [SerializeField] private ForgeView _forgeView;
        [SerializeField] private AltarView _altarView;
        [SerializeField] private GuildView _guildView;
        [SerializeField] private SanctuaryView _sanctuaryView;
        [SerializeField] private ChallengeTowerView _challengeTowerView;
        [SerializeField] private VoyageView _voyageView;
        [SerializeField] private FortuneWheelView _wheelFortuneView;
        [SerializeField] private MagicCircleView _magicCircleView;
        [SerializeField] private MinesView _minesView;
        [SerializeField] private ArenaView _arenaView;
        [SerializeField] private PetsZooView _petsZooView;
        [SerializeField] private TravelCircleView _travelCircleView;
        [SerializeField] private TaskboardView _taskboardView;
        [SerializeField] private LongTravelView _longTravelView;
        
        public override void Install(IContainerBuilder builder)
        {
            var canvas = Instantiate(_canvas);
            canvas.gameObject.name = MAIN_PANELS;
            canvas.GetComponent<Canvas>().sortingOrder = CANVAS_ORDER;
            builder.RegisterInstance(canvas);

            var safeArea = Instantiate(_safeArea, canvas.transform);
            //builder.RegisterInstance(safeArea);

            builder.RegisterUiView<TavernController, TavernView>(_tavernView, safeArea.RootTemporallyWindows);
            builder.RegisterUiView<MarketController, MarketView>(_marketView, safeArea.RootTemporallyWindows);
            builder.RegisterUiView<ForgeController, ForgeView>(_forgeView, safeArea.RootTemporallyWindows);
            builder.RegisterUiView<AltarController, AltarView>(_altarView, safeArea.RootTemporallyWindows);
            builder.RegisterUiView<GuildController, GuildView>(_guildView, safeArea.RootTemporallyWindows);
            builder.RegisterUiView<SanctuaryController, SanctuaryView>(_sanctuaryView, safeArea.RootTemporallyWindows);
            builder.RegisterUiView<ChallengeTowerController, ChallengeTowerView>(_challengeTowerView, safeArea.RootTemporallyWindows);
            builder.RegisterUiView<VoyageController, VoyageView>(_voyageView, safeArea.RootTemporallyWindows);
            builder.RegisterUiView<FortuneWheelController, FortuneWheelView>(_wheelFortuneView, safeArea.RootTemporallyWindows);
            builder.RegisterUiView<MagicCircleController, MagicCircleView>(_magicCircleView, safeArea.RootTemporallyWindows);
            builder.RegisterUiView<LongTravelController, LongTravelView>(_longTravelView, safeArea.RootTemporallyWindows);
            builder.RegisterUiView<MinesPageController, MinesView>(_minesView, safeArea.RootTemporallyWindows);
            builder.RegisterUiView<ArenaController, ArenaView>(_arenaView, safeArea.RootTemporallyWindows);
            builder.RegisterUiView<PetsZooController, PetsZooView>(_petsZooView, safeArea.RootTemporallyWindows);
            builder.RegisterUiView<TravelCircleController, TravelCircleView>(_travelCircleView, safeArea.RootTemporallyWindows);
            builder.RegisterUiView<TaskboardController, TaskboardView>(_taskboardView, safeArea.RootTemporallyWindows);
        }
    }
}
