using Common.Inventories.Splinters;
using System;

namespace UIController.Rewards
{
    [Serializable]
    public class PosibleRewardSplinter : PosibleRewardObject
    {
        public SplinterName ID = SplinterName.OneStarPeople;
        public GameSplinter GetSplinter { get { return new GameSplinter($"{ID}", 1); } }
        public SplinterName GetID { get => ID; }

    }
}