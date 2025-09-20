using Common;
using Newtonsoft.Json;
using System;
using Common.Db.CommonDictionaries;

namespace Models.Data.Inventories
{
    public class InventoryBaseItem : BaseDataModel
    {
        [NonSerialized][JsonIgnore] public CommonDictionaries CommonDictionaries;

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
