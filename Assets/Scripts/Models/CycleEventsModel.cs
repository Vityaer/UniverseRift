namespace Models
{
    [System.Serializable]
    public class CycleEventsModel : SimpleBuildingModel
    {
        public MonthlyRequirementsModel monthlyRequirements = new MonthlyRequirementsModel();
    }
}
