using City.Buildings.PageCycleEvent.MonthlyEvents;
using Models.Data;
using System;
using System.Collections.Generic;

namespace Models
{
    [Serializable]
    public class ListRequirementModel : BaseModel
    {
        public int ID;
        public List<AchievmentData> list = new List<AchievmentData>();
        public ListRequirementModel(TypeMonthlyTasks type)
        {
            ID = (int)type;
        }
    }
}
