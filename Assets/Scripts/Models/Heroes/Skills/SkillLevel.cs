using Models.Heroes.Actions;
using System.Collections.Generic;

namespace Models.Heroes.Skills
{
    [System.Serializable]
    public class SkillLevel
    {
        public int requireNumBreakthrough;
        public List<Effect> effects = new List<Effect>();
    }
}