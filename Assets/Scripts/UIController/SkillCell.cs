using Models.Heroes.Skills;
using UnityEngine;
using UnityEngine.UI;

namespace UIController
{
    public class SkillCell : MonoBehaviour
    {
        public GameObject mainPanel;
        public Text textNameSkill;
        public Text textDescriptionSkill;
        public Text textLevelSkill;
        public Image ImageSkill;
        public Skill Skill;

        public void ShowSkill(Skill skill)
        {
            this.Skill = skill;
            textNameSkill.text = skill.Name;
            textDescriptionSkill.text = skill.Description;
            textLevelSkill.text = string.Concat("Уровень ", (skill.Level + 1).ToString());
            ImageSkill.sprite = skill.Icon;
            mainPanel.SetActive(true);
        }

        public void Close()
        {
            mainPanel.SetActive(false);
        }
    }
}