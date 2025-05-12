using System.Collections.Generic;
using Models.City.AbstactBuildingModels;
using Models.Data.Inventories;

namespace Models.City.Sanctuaries
{
    public class SanctuaryBuildingModel : BuildingModel
    {
        public List<ResourceData> SimpleReplaceResource = new();
        public List<ResourceData> ConcreteReplaceResource = new();
    }
}