using Common;
using Common.Resourses;
using Models.Common.BigDigits;

namespace Models.Data.Inventories
{
    [System.Serializable]
    public class ResourceData : InventoryBaseItem
    {
        public ResourceType Type;
        public BigDigit Amount = new BigDigit();

        public override BaseObject CreateGameObject()
        {
            return new GameResource(Type, Amount);
        }
    }
}
