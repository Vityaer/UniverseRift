using Altar;
using City.Buildings.Altar;
using City.Buildings.Arena;
using City.Buildings.Forge;
using City.Buildings.Guild;
using City.Buildings.MagicCircle;
using City.Buildings.Market;
using City.Buildings.Market.CityMarkets;
using City.Buildings.Mines;
using City.Buildings.Pets;
using City.Buildings.Sanctuary;
using City.Buildings.TaskGiver;
using City.Buildings.Tavern;
using City.Buildings.Tower;
using City.Buildings.TravelCircle;
using City.Buildings.Voyage;
using City.Buildings.WheelFortune;
using UiExtensions.MainPages;
using UniRx;
using UnityEngine;
using VContainer;
using VContainer.Unity;

namespace MainPages.City
{
    public class CityPageController : UiMainPageController<CityPageView>, IInitializable
    {
        [Inject] private readonly TavernController m_tavernController;
        [Inject] private readonly MarketController m_marketController;
        [Inject] private readonly ForgeController m_forgeController;
        [Inject] private readonly AltarController m_altarController;
        [Inject] private readonly GuildController m_guildController;
        [Inject] private readonly SanctuaryController m_sanctuaryController;
        [Inject] private readonly FortuneWheelController m_fortuneWheelController;
        [Inject] private readonly PetsZooController m_petsZooController;
        [Inject] private readonly TaskboardController m_taskboardController;

        public new void Initialize()
        {
            RegisterBuilding(m_tavernController, View.Tavern);
            RegisterBuilding(m_marketController, View.Market);
            RegisterBuilding(m_forgeController, View.Forge);
            RegisterBuilding(m_altarController, View.Altar);
            //RegisterBuilding(_guildController, View.Guild);
            RegisterBuilding(m_sanctuaryController, View.Sanctuary);
            RegisterBuilding(m_fortuneWheelController, View.Wheel);
            RegisterBuilding(m_petsZooController, View.PetZoo);
            RegisterBuilding(m_taskboardController, View.Taskboard);


            View.Guild.BuildingButton.OnClickAsObservable().Subscribe(_ => m_guildController.Open())
                .AddTo(Disposables);
        }


    }
}
