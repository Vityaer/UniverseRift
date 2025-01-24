using Db.CommonDictionaries;
using Models.City.AbstactBuildingModels;
using Models.Data.Inventories;
using System.Collections.Generic;
using UIController.Rewards.PosibleRewards;

namespace Models.City.MagicCircles
{
    public class MagicCircleBuildingModel : BuildingModel
    {
        public ResourceData HireCost;
        public Dictionary<string, float> SubjectChances = new();
        public PosibleRewardData PosibleRewardData = new();

        public override void SetCommonDictionary(CommonDictionaries commonDictionaries)
        {
            base.SetCommonDictionary(commonDictionaries);
            PosibleRewardData.SetCommonDictionaries(commonDictionaries);
        }
    }
}
