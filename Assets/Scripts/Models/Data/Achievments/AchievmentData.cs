namespace Models.Data
{
    [System.Serializable]
    public class AchievmentData : BaseDataModel
    {
        public int Id;
        public int PlayerId;
        public string ModelId;
        public int CurrentStage;
        public float Amount;
        public int E10;
        public bool IsComplete;
    }
}
