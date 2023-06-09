using Models.Heroes.Skills;
using UIController;
using UnityEngine;
using UnityEngine.UI;

namespace City.TrainCamp
{
    public class SkillUI : MonoBehaviour
    {
        public Image image;
        public Skill skill;
        public SkillInfoController detailController;

        public void SetInfo(Skill skill)
        {
            this.skill = skill;
            image.sprite = this.skill.image;
            gameObject.SetActive(true);
        }

        public void OpenDetail()
        {
            detailController.ShowSkill(skill);
        }
        public void OffObject()
        {
            skill = null;
            gameObject.SetActive(false);
        }
    }
}