namespace City.Buildings.Requirements
{
    public class DailyTaskAchievmentView : AchievmentView
    {
        protected override void GiveReward()
        {
            Data.GiveReward();
        }
    }
}