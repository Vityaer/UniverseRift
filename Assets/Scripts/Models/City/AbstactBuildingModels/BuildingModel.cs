using Db.CommonDictionaries;

namespace Models.City.AbstactBuildingModels
{
    public class BuildingModel : BaseModel
    {
        protected CommonDictionaries CommonDictionaries;

        public string name;

        public virtual void SetCommonDictionary(CommonDictionaries commonDictionaries)
        {
            CommonDictionaries = commonDictionaries;
        }
    }
}
