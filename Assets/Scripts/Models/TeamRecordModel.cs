namespace Models
{
    [System.Serializable]
    public class TeamRecordModel : BaseRecordModel
    {
        public TeamFightData value;

        public TeamRecordModel(string key, TeamFightData value)
        {
            this.key = key;
            this.value = value.Clone();
        }

        public TeamRecordModel(string key)
        {
            this.key = key;
            value = new TeamFightData();
        }

        public void SetNewTeam(TeamFightData newTeam)
        {
            this.value = newTeam;
        }
    }
}
