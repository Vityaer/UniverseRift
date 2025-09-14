using UIController.Rewards;

namespace Models.Data.Rewards
{
    public class BossWinRewardModel
    {
        public int StartIndex;
        public int EndIndex;
        public RewardModel Reward;

        public BossWinRewardModel(RewardModel reward)
        {
            Reward = reward;
        }
    }
}