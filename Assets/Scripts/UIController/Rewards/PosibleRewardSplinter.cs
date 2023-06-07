using System;
using UIController.Inventory;

namespace UIController.Rewards
{
    [Serializable]
    public class PosibleRewardSplinter : PosibleRewardObject
    {
        public SplinterName ID = SplinterName.OneStarPeople;
        public SplinterModel GetSplinter { get { return new SplinterModel($"{ID}", 1); } }
        public SplinterName GetID { get => ID; }

    }
}