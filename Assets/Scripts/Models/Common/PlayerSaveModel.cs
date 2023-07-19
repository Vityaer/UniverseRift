using Models.Data.Heroes;
using Models.Data.Inventories;
using Models.Data.Players;
using System.Collections.Generic;

namespace Models.Common
{
    public class PlayerSaveModel
    {
        public CityData City = new CityData();
        public PlayerData Player = new PlayerData();
        public CycleEventsData CycleEventsData = new CycleEventsData();
        public HeroesStorage HeroesStorage = new HeroesStorage();
        public InventoryData InventoryData = new InventoryData();
        public List<ResourceData> Resources = new List<ResourceData>();
    }
}
