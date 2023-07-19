using City.Buildings.Arena;
using City.Buildings.MagicCircle;
using City.Buildings.Mines;
using City.Buildings.Tower;
using City.Buildings.TravelCircle;
using City.Buildings.Voyage;
using UiExtensions.MainPages;
using VContainer.Unity;

namespace MainPages.SecondCity
{
    public class SecondCityPageController : UiMainPageController<SecondCityPageView>, IInitializable
    {
        public new void Initialize()
        {
            OpenBuildingOnClick<ChallengeTowerController>(View.TowerDeathButton);
            OpenBuildingOnClick<VoyageController>(View.VoyageButton);
            OpenBuildingOnClick<MagicCircleController>(View.MagicCircleButton);
            OpenBuildingOnClick<MinesController>(View.MinesButton);
            OpenBuildingOnClick<ArenaController>(View.ArenaButton);
            OpenBuildingOnClick<TravelCircleController>(View.TravelCirleButton);
        }
    }
}
