using Common.Inventories.Splinters;

namespace Models.Data.Inventories
{
    public class SplinterData : BaseDataModel
    {
        public string Id;
        public int Amount;

        public SplinterData() { }

        public SplinterData(GameSplinter splinter)
        {
            Id = splinter.Id;
            Amount = splinter.Amount;
        }
    }
}
