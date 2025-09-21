using Altar;
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
        [Inject] private readonly TavernController _tavernController;
        [Inject] private readonly MarketController _marketController;
        [Inject] private readonly ForgeController _forgeController;
        [Inject] private readonly AltarController _altarController;
        [Inject] private readonly GuildController _guildController;
        [Inject] private readonly SanctuaryController _sanctuaryController;
        [Inject] private readonly FortuneWheelController _fortuneWheelController;
        [Inject] private readonly PetsZooController _petsZooController;
        [Inject] private readonly TaskboardController _taskboardController;

        public new void Initialize()
        {
            View.BackgroundCanvas.worldCamera = Camera.main;
            RegisterBuilding(_tavernController, View.Tavern);
            RegisterBuilding(_marketController, View.Market);
            RegisterBuilding(_forgeController, View.Forge);
            RegisterBuilding(_altarController, View.Altar);
            //RegisterBuilding(_guildController, View.Guild);
            RegisterBuilding(_sanctuaryController, View.Sanctuary);
            RegisterBuilding(_fortuneWheelController, View.Wheel);
            RegisterBuilding(_petsZooController, View.PetZoo);
            RegisterBuilding(_taskboardController, View.Taskboard);


            View.Guild.BuildingButton.OnClickAsObservable().Subscribe(_ => _guildController.Open()).AddTo(Disposables);
        }


    }
}
