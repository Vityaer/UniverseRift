using System.Collections.Generic;
using Common.Resourses;
using Sirenix.OdinInspector;

namespace Models.Misc.Helps
{
    public class HelpResourceModel : BaseModel
    {
        public ResourceType TargetType;
        
        public List<PageType> PageTypes = new();
    }
}