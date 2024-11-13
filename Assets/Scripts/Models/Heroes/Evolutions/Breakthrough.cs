using Models.Heroes.HeroCharacteristics;
using UnityEngine;

namespace Models.Heroes.Evolutions
{
    [System.Serializable]
    public class Breakthrough
    {
        [Header("Data")]
        public int NumBreakthrough;

        [Header("Require")]
        public int RequireLevel;

        [Header("Reward")]
        public int NewLimitLevel;
        public IncreaseCharacteristicsModel IncCharacts;
    }
}