using Models.City.AbstactBuildingModels;
using Models.Data.Inventories;
using System.Collections.Generic;

namespace Models.City.MagicCircles
{
    public class MagicCircleBuildingModel : BuildingModel
    {
        public ResourceData HireCost;
        public Dictionary<string, float> SubjectChances = new();
        public List<string> ItemIds = new();
        public List<string> SplinterIds = new();

    }
}
