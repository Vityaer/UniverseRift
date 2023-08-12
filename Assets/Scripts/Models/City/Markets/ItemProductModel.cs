using Db.CommonDictionaries;
using Models.Data.Inventories;

namespace Models.City.Markets
{
    [System.Serializable]
    public class ItemProductModel : BaseProductModel
    {
        public new ItemData Subject = new ItemData();

        public ItemProductModel()
        {
            Subject = new ItemData();
        }

        public ItemProductModel(CommonDictionaries commonDictionaries)
        {
            Subject.CommonDictionaries = commonDictionaries;
        }

    }
}
