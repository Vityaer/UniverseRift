using City.Buildings.Arena;
using City.Buildings.LongTravels;
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
        [Inject] private readonly ChallengeTowerController m_challengeTowerController;
        [Inject] private readonly MagicCircleController m_magicCircleController;
        [Inject] private readonly MinesPageController m_minesController;
        [Inject] private readonly ArenaController m_arenaController;
        [Inject] private readonly VoyageController m_voyageController;
        [Inject] private readonly TravelCircleController m_travelCircleController;
        [Inject] private readonly LongTravelController m_longTravelController;

        public new void Initialize()
        {
            RegisterBuilding(m_challengeTowerController, View.TowerDeath);
            RegisterBuilding(m_magicCircleController, View.MagicCircle);
            RegisterBuilding(m_minesController, View.Mines);
            RegisterBuilding(m_arenaController, View.Arena);
            RegisterBuilding(m_voyageController, View.Voyage);
            RegisterBuilding(m_travelCircleController, View.TravelCirle);
            RegisterBuilding(m_longTravelController, View.RaidTravels);
        }
    }
}
