using City.TaskBoard;
using System;

namespace Models.Data
{
    public class TaskData : BaseDataModel
    {
        public string TaskId;
        public TaskStatusType Status;
        public string DateTimeStart;
    }
}
