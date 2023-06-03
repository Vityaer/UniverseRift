using System;
using System.Collections.Generic;

namespace Models
{
    [Serializable]
    public class PlayerModel : BaseModel
    {
        public ListResource storeResources = new ListResource();
        public List<Task> listTasks = new List<Task>();
        public RequirementStorageModel allRequirement = new RequirementStorageModel();
    }
}
