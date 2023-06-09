namespace Models
{
    [System.Serializable]
    public class TeamRecordModel : BaseRecordModel
    {
        public TeamFightModel value;
        public TeamRecordModel(string key, TeamFightModel value)
        {
            this.key = key;
            this.value = value.Clone();
        }
        public TeamRecordModel(string key)
        {
            this.key = key;
            value = new TeamFightModel();
        }
        public void SetNewTeam(TeamFightModel newTeam)
        {
            this.value = newTeam;
        }
    }
}
