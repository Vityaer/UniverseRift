using System;
using System.Collections.Generic;

namespace Models
{
    [Serializable]
    public class TaskGiverModel : BaseModel
    {
        public List<TaskModel> tasks = new List<TaskModel>();
    }
}
