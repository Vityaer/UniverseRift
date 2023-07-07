using Altar;
using City.Buildings.Arena;
using City.Buildings.CityButtons;
using City.Buildings.CityButtons.EventAgent;
using City.Buildings.Forge;
using City.Buildings.Friends;
using City.Buildings.Guild;
using City.Buildings.MagicCircle;
using City.Buildings.Mails;
using City.Buildings.Market;
using City.Buildings.Mines;
using City.Buildings.Pets;
using City.Buildings.PlayerPanels;
using City.Buildings.Sanctuary;
using City.Buildings.TaskGiver;
using City.Buildings.Tavern;
using City.Buildings.Tower;
using City.Buildings.TravelCircle;
using City.Buildings.Voyage;
using City.Buildings.WheelFortune;
using UiExtensions.MainPages;
using VContainer.Unity;

namespace MainPages.City
{
    public class CityPageController : UiMainPageController<CityPageView>, IInitializable
    {
        public void Initialize()
        {
            OpenBuildingOnClick<TavernController>(View.TavernButton);
            OpenBuildingOnClick<MarketController>(View.MarketButton);
            OpenBuildingOnClick<ForgeController>(View.ForgeButton);
            OpenBuildingOnClick<AltarController>(View.AltarButton);
            OpenBuildingOnClick<GuildController>(View.GuildButton);
            OpenBuildingOnClick<SanctuaryController>(View.SanctuaryButton);
            OpenBuildingOnClick<ChallengeTowerController>(View.TowerDeathButton);
            OpenBuildingOnClick<VoyageController>(View.VoyageButton);
            OpenBuildingOnClick<WheelFortuneController>(View.WheelButton);
            OpenBuildingOnClick<MagicCircleController>(View.MagicCircleButton);
            OpenBuildingOnClick<MinesController>(View.MinesButton);
            OpenBuildingOnClick<ArenaController>(View.ArenaButton);
            OpenBuildingOnClick<PetsZooController>(View.PetZooButton);
            OpenBuildingOnClick<TravelCircleController>(View.TravelCirleButton);
            OpenBuildingOnClick<TaskboardController>(View.TaskboardButton);

        }
    }
}
