using Models.Heroes.Actions;
using Sirenix.OdinInspector;
using System.Collections.Generic;

namespace Models.Heroes.Skills
{
    public class SkillLevel
    {
        [LabelWidth(200)] public int RequireNumBreakthrough;
        public List<Effect> Effects = new();
    }
}