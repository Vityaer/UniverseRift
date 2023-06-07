using Models.Fights.Campaign;

namespace Models.Fights.Challenge
{
    [System.Serializable]
    public class ChallengeModel : BaseModel
    {
        public string Name;
        public MissionModel Mission;
        public bool IsDone;
    }
}