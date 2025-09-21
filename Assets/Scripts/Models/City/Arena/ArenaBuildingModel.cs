using System.Collections.Generic;
using Models.Arenas;
using Models.City.AbstactBuildingModels;

namespace Models.City.Arena
{
    public class ArenaBuildingModel : BuildingModel
    {
		public Dictionary<ArenaType, ArenaContainerModel> ArenaContainers = new();
    }
}