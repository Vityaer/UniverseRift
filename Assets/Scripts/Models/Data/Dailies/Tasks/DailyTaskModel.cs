using Models.Achievments;
using System.Collections.Generic;

namespace Models.Data.Dailies.Tasks
{
    public class DailyTaskModel : BaseModel
    {
        public List<AchievmentModel> Tasks = new List<AchievmentModel>();
    }
}
