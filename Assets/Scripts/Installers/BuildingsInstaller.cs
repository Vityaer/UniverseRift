using Altar;
using City.Buildings.Altar;
using City.Buildings.Arena;
using City.Buildings.Forge;
using City.Buildings.Friends;
using City.Buildings.Guild;
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
        [SerializeField] private TavernView _tavernView;
        [SerializeField] private MarketView _marketView;
        [SerializeField] private ForgeView _forgeView;
        [SerializeField] private AltarView _altarView;
        [SerializeField] private GuildView _guildView;
        [SerializeField] private SanctuaryView _sanctuaryView;
        [SerializeField] private ChallengeTowerView _challengeTowerView;
        [SerializeField] private VoyageView _voyageView;
        [SerializeField] private WheelFortuneView _wheelFortuneView;
        [SerializeField] private MagicCircleView _magicCircleView;
        //[SerializeField] private MinesView _minesView;
        [SerializeField] private ArenaView _arenaView;
        [SerializeField] private PetsZooView _petsZooView;
        [SerializeField] private TravelCircleView _travelCircleView;
        [SerializeField] private TaskboardView _taskboardView;
        //[SerializeField] private FriendsView _friendsView;
        //[SerializeField] private MailView _mailView;

        public override void Install(IContainerBuilder builder)
        {
            var canvas = Instantiate(_canvas);
            canvas.gameObject.name = MAIN_PANELS;
            canvas.GetComponent<Canvas>().sortingOrder = CANVAS_ORDER;
            builder.RegisterInstance(canvas);

            builder.RegisterUiView<TavernController, TavernView>(_tavernView, canvas.transform);
            builder.RegisterUiView<MarketController, MarketView>(_marketView, canvas.transform);
            builder.RegisterUiView<ForgeController, ForgeView>(_forgeView, canvas.transform);
            builder.RegisterUiView<AltarController, AltarView>(_altarView, canvas.transform);
            builder.RegisterUiView<GuildController, GuildView>(_guildView, canvas.transform);
            builder.RegisterUiView<SanctuaryController, SanctuaryView>(_sanctuaryView, canvas.transform);
            builder.RegisterUiView<ChallengeTowerController, ChallengeTowerView>(_challengeTowerView, canvas.transform);
            builder.RegisterUiView<VoyageController, VoyageView>(_voyageView, canvas.transform);
            builder.RegisterUiView<WheelFortuneController, WheelFortuneView>(_wheelFortuneView, canvas.transform);
            builder.RegisterUiView<MagicCircleController, MagicCircleView>(_magicCircleView, canvas.transform);
            //builder.RegisterUiView<MinesController, MinesView>(_minesView, canvas.transform);
            builder.RegisterUiView<ArenaController, ArenaView>(_arenaView, canvas.transform);
            builder.RegisterUiView<PetsZooController, PetsZooView>(_petsZooView, canvas.transform);
            builder.RegisterUiView<TravelCircleController, TravelCircleView>(_travelCircleView, canvas.transform);
            builder.RegisterUiView<TaskboardController, TaskboardView>(_taskboardView, canvas.transform);
            //builder.RegisterUiView<FriendsController, FriendsView>(_friendsView, canvas.transform);
            //builder.RegisterUiView<MailController, MailView>(_mailView, canvas.transform);
        }
    }
}
