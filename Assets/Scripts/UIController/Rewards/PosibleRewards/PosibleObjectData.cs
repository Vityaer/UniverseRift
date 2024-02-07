using Common;
using Common.Inventories.Splinters;
using Db.CommonDictionaries;
using Models.Data.Inventories;
using Newtonsoft.Json;

namespace UIController.Rewards.PosibleRewards
{
    public class PosibleObjectData
    {
        public int Posibility;

        public virtual BaseObject CreateGameObject()
        {
            return null;
        }
    }

    public class PosibleObjectData<T> : PosibleObjectData
        where T : InventoryBaseItem
    {
        public T Value;
        [JsonIgnore] public CommonDictionaries CommonDictionaries;

        public override BaseObject CreateGameObject()
        {
            Value.SetCommonDictionaries(CommonDictionaries);
            var result = Value.CreateGameObject();

            result.Amount = 0;
            return result;
        }
    }
}