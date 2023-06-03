using System;
using System.Collections.Generic;

namespace Models
{
    //Tasks
    [Serializable]
    public class TaskGiverModel : BaseModel
    {
        public List<Task> tasks = new List<Task>();
    }
}
