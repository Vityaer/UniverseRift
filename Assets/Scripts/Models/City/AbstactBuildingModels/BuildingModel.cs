using Common.Db.CommonDictionaries;

namespace Models.City.AbstactBuildingModels
{
    public class BuildingModel : BaseModel
    {
        protected CommonDictionaries CommonDictionaries;

        public virtual void SetCommonDictionary(CommonDictionaries commonDictionaries)
        {
            CommonDictionaries = commonDictionaries;
        }
    }
}
