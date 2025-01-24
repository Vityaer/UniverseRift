using Models.City.AbstactBuildingModels;
using Models.City.Mines.Energies;
using System.Collections.Generic;

namespace Models.City.Mines
{
    public class MineBuildingModel : BuildingModel
    {
        public List<MineEnergyDataModel> MineEnergyDatas = new();
        public List<MineSettingsCampaignContainer> SettingsCampaigns = new();
    }
}
