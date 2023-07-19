using Common;

namespace Models.Data.Inventories
{
    public class InventoryBaseItem : BaseDataModel
    {
        public virtual BaseObject CreateGameObject()
        {
            return null;
        }
    }
}
