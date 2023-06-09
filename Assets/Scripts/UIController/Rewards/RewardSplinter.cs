using UIController.Inventory;

namespace UIController.Rewards
{
    [System.Serializable]
    public class RewardSplinter
    {
        public SplinterName ID = SplinterName.OneStarPeople;
        public int amount = 1;
        private Splinter splinter = null;
        public Splinter GetSplinter { get { if (splinter == null) { splinter = new Splinter($"{ID}", amount); } return splinter; } }

        public RewardSplinter(SplinterName ID, int amount)
        {
            this.ID = ID;
            this.amount = amount;
        }

        public RewardSplinter(Splinter splinter)
        {
            ID = SplinterName.OneStarPeople;
            this.splinter = splinter;
        }

        public RewardSplinter Clone()
        {
            return new RewardSplinter(ID, amount);
        }

    }
}