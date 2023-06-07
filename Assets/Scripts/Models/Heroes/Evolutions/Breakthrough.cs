using Models.Heroes.Characteristics;
using UnityEngine;

namespace Models.Heroes.Evolutions
{
    [System.Serializable]
    public class Breakthrough
    {
        [Header("Data")]
        public uint NumBreakthrough;

        [Header("Require")]
        public uint RequireLevel;

        [Header("Reward")]
        public uint NewLimitLevel;
        public IncreaseCharacteristicsModel IncCharacts;

        [Header("Serious changes")]
        public bool IsSeriousChange = false;
        public string NewName = string.Empty;
        public string NewRace;
        public string NewClassHero;
        public string NewModelId;

    }
}