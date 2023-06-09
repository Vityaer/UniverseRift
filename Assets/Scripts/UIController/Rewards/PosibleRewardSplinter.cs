using System;
using UIController.Inventory;

namespace UIController.Rewards
{
    [Serializable]
    public class PosibleRewardSplinter : PosibleRewardObject
    {
        public SplinterName ID = SplinterName.OneStarPeople;
        public Splinter GetSplinter { get { return new Splinter($"{ID}", 1); } }
        public SplinterName GetID { get => ID; }

    }
}