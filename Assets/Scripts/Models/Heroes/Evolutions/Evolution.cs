using Models.Heroes.HeroCharacteristics;
using Sirenix.Utilities;
using System;
using System.Collections.Generic;

namespace Models.Heroes.Evolutions
{
    [System.Serializable]
    public class Evolution
    {
        public int CurrentBreakthrough = 0;
        public List<Breakthrough> Stages = new List<Breakthrough>();

        public int LimitLevel() => Stages[CurrentBreakthrough].NewLimitLevel;

        public bool OnLevelUp(int level)
        {
            bool result = false;
            if (CurrentBreakthrough + 1 < Stages.Count)
            {
                if (Stages[CurrentBreakthrough + 1].RequireLevel == level)
                {
                    CurrentBreakthrough++;
                    result = true;
                }
            }
            return result;
        }

        public IncreaseCharacteristicsModel GetGrowth(int rating)
        {
            return Stages[rating - 1].IncCharacts;
        }

        public Evolution Clone()
        {
            return new Evolution()
            {
                CurrentBreakthrough = this.CurrentBreakthrough,
                Stages = this.Stages
            };
        }
    }
}