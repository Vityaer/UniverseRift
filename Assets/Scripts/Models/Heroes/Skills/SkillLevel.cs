using Models.Heroes.Actions;
using System.Collections.Generic;

namespace Models.Heroes.Skills
{
    public class SkillLevel
    {
        public int RequireNumBreakthrough;
        public List<Effect> Effects = new();
    }
}