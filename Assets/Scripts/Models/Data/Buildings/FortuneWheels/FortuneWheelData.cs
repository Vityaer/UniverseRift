using System.Collections.Generic;

namespace Models.Data.Buildings.FortuneWheels
{
    public class FortuneWheelData
    {
        public int RefreshCount;
        public List<FortuneRewardData> Rewards = new();
    }
}
