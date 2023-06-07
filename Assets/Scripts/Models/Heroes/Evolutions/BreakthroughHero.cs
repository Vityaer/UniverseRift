using Models.Heroes.Characteristics;
using Sirenix.Utilities;
using System.Collections.Generic;

namespace Models.Heroes.Evolutions
{
    [System.Serializable]
    public class BreakthroughHero
    {
        public int currentBreakthrough = 0;
        public List<Breakthrough> listBreakthrough = new List<Breakthrough>();

        public int LimitLevel => (int)listBreakthrough[currentBreakthrough].NewLimitLevel;

        public bool OnLevelUp(int level)
        {
            bool result = false;
            if (currentBreakthrough + 1 < listBreakthrough.Count)
            {
                if (listBreakthrough[currentBreakthrough + 1].RequireLevel == level)
                {
                    currentBreakthrough++;
                    result = true;
                }
            }
            return result;
        }

        public IncreaseCharacteristicsModel GetGrowth(int rating)
        {
            return listBreakthrough[rating].IncCharacts;
        }

        public void ChangeData(GeneralInfoHero generalInfo)
        {
            Breakthrough evolve = listBreakthrough[currentBreakthrough];
            if (evolve.IsSeriousChange)
            {
                if (!evolve.NewName.IsNullOrWhitespace()) generalInfo.Name = evolve.NewName;
                if (!evolve.NewModelId.IsNullOrWhitespace()) generalInfo.ViewId = evolve.NewModelId;
                generalInfo.Race = evolve.NewRace;
                generalInfo.ClassHero = evolve.NewClassHero;
            }
        }
    }
}