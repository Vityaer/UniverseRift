using City.Buildings.Arena;
using City.Buildings.MagicCircle;
using City.Buildings.Mines;
using City.Buildings.Tower;
using City.Buildings.TravelCircle;
using City.Buildings.Voyage;
using UiExtensions.MainPages;
using VContainer;
using VContainer.Unity;

namespace MainPages.SecondCity
{
    public class SecondCityPageController : UiMainPageController<SecondCityPageView>, IInitializable
    {
        [Inject] private readonly ChallengeTowerController _challengeTowerController;
        [Inject] private readonly MagicCircleController _magicCircleController;
        [Inject] private readonly MinesPageController _minesController;
        [Inject] private readonly ArenaController _arenaController;
        [Inject] private readonly VoyageController _voyageController;
        [Inject] private readonly TravelCircleController _travelCircleController;

        public new void Initialize()
        {
            RegisterBuilding(_challengeTowerController, View.TowerDeath);
            RegisterBuilding(_magicCircleController, View.MagicCircle);
            RegisterBuilding(_minesController, View.Mines);
            RegisterBuilding(_arenaController, View.Arena);
            RegisterBuilding(_voyageController, View.Voyage);
            RegisterBuilding(_travelCircleController, View.TravelCirle);
        }
    }
}
