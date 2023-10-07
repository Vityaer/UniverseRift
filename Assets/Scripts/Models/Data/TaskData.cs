using City.TaskBoard;
using System;

namespace Models.Data
{
    public class TaskData : BaseDataModel
    {
        public int TaskId;
        public string TaskModelId;
        public TaskStatusType Status;
        public string DateTimeStart;
    }
}
