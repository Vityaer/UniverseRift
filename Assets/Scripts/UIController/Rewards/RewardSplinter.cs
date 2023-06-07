using UIController.Inventory;

namespace UIController.Rewards
{
    [System.Serializable]
    public class RewardSplinter
    {
        public SplinterName ID = SplinterName.OneStarPeople;
        public int amount = 1;
        private SplinterModel splinter = null;
        public SplinterModel GetSplinter { get { if (splinter == null) { splinter = new SplinterModel($"{ID}", amount); } return splinter; } }

        public RewardSplinter(SplinterName ID, int amount)
        {
            this.ID = ID;
            this.amount = amount;
        }

        public RewardSplinter(SplinterModel splinter)
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