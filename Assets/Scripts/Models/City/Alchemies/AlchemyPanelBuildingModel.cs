using System.Collections.Generic;
using Common.Db.CommonDictionaries;
using Models.City.AbstactBuildingModels;

namespace Models.City.Alchemies
{
    public class AlchemyPanelBuildingModel : BuildingModel
    {
        public List<string> Products = new List<string>();

        public AlchemyPanelBuildingModel(CommonDictionaries dictionaries)
        {
            SetCommonDictionary(dictionaries);
        }
    }
}