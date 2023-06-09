using City.Acievements;
using Common;

namespace Models.Requiremets
{
    [System.Serializable]
    public class AchievementSave
    {
        public int ID;
        public int currentStage;
        public BigDigit progress;

        public AchievementSave(Achievement requirement)
        {
            ChangeData(requirement);
        }

        public void ChangeData(Achievement requirement)
        {
            this.ID = requirement.ID;
            this.currentStage = requirement.CurrentStage;
            this.progress = requirement.Progress;
        }
    }
}
