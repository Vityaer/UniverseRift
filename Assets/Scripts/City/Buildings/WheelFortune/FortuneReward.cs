using Common;
using System;

namespace City.Buildings.WheelFortune
{
    [Serializable]
    public class FortuneReward<T> : FortuneRewardModel where T : BaseObject
    {
        public T subject;
    }
}