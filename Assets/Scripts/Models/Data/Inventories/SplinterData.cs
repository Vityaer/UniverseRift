using Common;
using Common.Inventories.Splinters;

namespace Models.Data.Inventories
{
    public class SplinterData : InventoryBaseItem
    {
        public string Id;
        public int Amount;

        public SplinterData() { }

        public SplinterData(GameSplinter splinter)
        {
            Id = splinter.Id;
            Amount = splinter.Amount;
        }

        public override BaseObject CreateGameObject()
        {
            return new GameSplinter(Id, Amount);
        }
    }
}
