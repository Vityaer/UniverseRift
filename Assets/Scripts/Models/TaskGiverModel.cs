using System;
using System.Collections.Generic;

namespace Models
{
    //Tasks
    [Serializable]
    public class TaskGiverModel : BaseModel
    {
        public List<TaskModel> tasks = new List<TaskModel>();
    }
}
