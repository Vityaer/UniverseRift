using UIController.Rewards;

namespace Models.Tasks
{
    [System.Serializable]

    public class GameTaskModel : BaseModel
    {
        public int Rating;
        public int RequireHour;
        public float FactorDelta;
        public RewardModel Reward = new();
    }
}


