using UnityEngine;

namespace Models
{
    [System.Serializable]
    public class BuildingWithFightTeamsModel : SimpleBuildingModel
    {
        [SerializeField] private TeamRecordsModel teamRecords = new TeamRecordsModel();
        public void SetRecordTeam(string name, TeamFightModel newTeam)
        {

        }
    }
}
