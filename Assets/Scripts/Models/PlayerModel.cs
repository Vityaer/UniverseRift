using Common.Resourses;
using System;
using System.Collections.Generic;

namespace Models
{
    [Serializable]
    public class PlayerModel : BaseModel
    {
        public ListResource storeResources = new ListResource();
        public List<TaskModel> listTasks = new List<TaskModel>();
        public RequirementStorageModel allRequirement = new RequirementStorageModel();
    }
}
