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

        [Header("Serious changes")]
        public bool IsSeriousChange = false;
        public string NewName = string.Empty;
        public string NewRace;
        public string NewClassHero;
        public string NewModelId;

    }
}