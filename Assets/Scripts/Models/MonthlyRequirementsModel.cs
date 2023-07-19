using City.Buildings.PageCycleEvent.MonthlyEvents;
using Models.Data;
using System.Collections.Generic;
using UnityEngine;

namespace Models
{
    [System.Serializable]
    public class MonthlyRequirementsModel : BaseModel
    {
        [SerializeField] private List<ListRequirementModel> listGroupRequirements = new List<ListRequirementModel>();
        public List<AchievmentData> GetTasks(TypeMonthlyTasks type)
        {
            List<AchievmentData> result = listGroupRequirements.Find(x => x.ID == ((int)type))?.list;
            if (result == null)
            {
                ListRequirementModel work = new ListRequirementModel(type);
                listGroupRequirements.Add(work);
                result = work.list;
            }
            return result;
        }
    }
}
