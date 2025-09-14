namespace City.Buildings.Requirement
{
    public class DailyTaskAchievmentView : AchievmentView
    {
        protected override void GiveReward()
        {
            Data.GiveReward();
        }
    }
}