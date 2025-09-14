using System.Collections.Generic;
using Common.Resourses;

namespace Models
{
    public class SweetEventData : AbstractCycleData
    {
        public ResourceType ResourceType;
        public Dictionary<string, string> ProductConcretes;
    }
}