using Common;
using Db.CommonDictionaries;
using System;

namespace Models.Data.Inventories
{
    public class InventoryBaseItem : BaseDataModel
    {
        [NonSerialized] public CommonDictionaries CommonDictionaries;

        public virtual BaseObject CreateGameObject()
        {
            return null;
        }

        public void SetCommonDictionaries(CommonDictionaries commonDictionaries)
        {
            CommonDictionaries = commonDictionaries;
        }
    }
}
