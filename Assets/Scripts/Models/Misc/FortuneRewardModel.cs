using Common;
using Models;
using System;

namespace City.Buildings.WheelFortune
{
    [Serializable]
    public abstract class FortuneRewardModel : BaseModel
    {
        public float Probability;
    }
}