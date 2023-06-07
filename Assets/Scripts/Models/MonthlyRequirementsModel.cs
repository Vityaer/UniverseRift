using Assets.Scripts.City.Buildings.PageCycleEvent.MonthlyEvents;
using Models.Requiremets;
using System.Collections.Generic;
using UnityEngine;

namespace Models
{
    [System.Serializable]
    public class MonthlyRequirementsModel : BaseModel
    {
        [SerializeField] private List<ListRequirementModel> listGroupRequirements = new List<ListRequirementModel>();
        public List<AchievementSave> GetTasks(TypeMonthlyTasks type)
        {
            List<AchievementSave> result = listGroupRequirements.Find(x => x.ID == ((int)type))?.list;
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
