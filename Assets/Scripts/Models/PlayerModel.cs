using Common.Resourses;
using Models.Data;
using System;
using System.Collections.Generic;

namespace Models
{
    [Serializable]
    public class PlayerModel : BaseModel
    {
        public List<ResourceData> storeResources = new();
        public List<TaskModel> listTasks = new List<TaskModel>();
        public RequirementStorageModel allRequirement = new RequirementStorageModel();
    }
}
