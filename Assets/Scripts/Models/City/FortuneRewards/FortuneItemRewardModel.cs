using City.Buildings.WheelFortune;
using Db.CommonDictionaries;
using Models.Data.Inventories;

namespace Models.City.FortuneRewards
{
    public class FortuneItemRewardModel : FortuneRewardModel
    {
        public new ItemData Subject = new ItemData();

        public FortuneItemRewardModel(CommonDictionaries commonDictionaries)
        {
            Subject.CommonDictionaries = commonDictionaries;
        }
    }
}
