using Common;
using UIController.Rewards;
using UnityEngine;

namespace City.Acievements
{
    [System.Serializable]
    public class RequirementStage
    {
        [Header("Requirement")]
        [SerializeField] private BigDigit requireCount;
        [Header("Reward")]
        public Reward reward;
        public BigDigit RequireCount { get => requireCount; }
    }
}