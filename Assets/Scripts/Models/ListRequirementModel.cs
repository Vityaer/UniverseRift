using City.Buildings.PageCycleEvent.MonthlyEvents;
using Models.Requiremets;
using System;
using System.Collections.Generic;

namespace Models
{
    [Serializable]
    public class ListRequirementModel : BaseModel
    {
        public int ID;
        public List<AchievementSave> list = new List<AchievementSave>();
        public ListRequirementModel(TypeMonthlyTasks type)
        {
            ID = (int)type;
        }
    }
}
