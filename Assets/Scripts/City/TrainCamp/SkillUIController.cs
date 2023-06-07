using Models.Heroes.Skills;
using System.Collections.Generic;
using UnityEngine;

namespace City.TrainCamp
{
    public class SkillUIController : MonoBehaviour
    {
        public List<SkillUI> skillsObject = new List<SkillUI>();

        public void ShowSkills(List<Skill> skills)
        {
            ClearPanelSkills();
            for (int i = 0; i < skills.Count; i++)
            {
                skillsObject[i].SetInfo(skills[i]);
            }
        }
        private void ClearPanelSkills()
        {
            foreach (var obj in skillsObject)
            {
                obj.OffObject();
            }
        }
    }
}