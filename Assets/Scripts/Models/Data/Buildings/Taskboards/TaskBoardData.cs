using System.Collections.Generic;

namespace Models.Data.Buildings.Taskboards
{
    public class TaskBoardData
    {
        public int TaskCountForCreate;
        public string CreateTasksLeftTime = string.Empty;
        public List<TaskData> ListTasks = new();
    }
}
