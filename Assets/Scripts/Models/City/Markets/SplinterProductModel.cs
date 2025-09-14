using Db.CommonDictionaries;
using Models.Data.Inventories;

namespace Models.City.Markets
{
    public class SplinterProductModel : BaseProductModel
    {
        public new SplinterData Subject = new SplinterData();

        public SplinterProductModel()
        {
            Subject = new SplinterData();
        }

        public SplinterProductModel(CommonDictionaries commonDictionaries)
        {
            Subject.CommonDictionaries = commonDictionaries;
        }
    }
}
