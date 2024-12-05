using Models.City.AbstactBuildingModels;
using Models.Data.Inventories;
using System.Collections.Generic;

namespace Models.City.MagicCircles
{
    public class MagicCircleBuildingModel : BuildingModel
    {
        public ResourceData HireCost;
        public Dictionary<string, float> SubjectChances = new();
        public Dictionary<string, float> Items = new();
        public Dictionary<string, float> Splinters = new();
    }
}
