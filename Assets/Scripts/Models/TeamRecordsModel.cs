using System.Collections.Generic;
using UnityEngine;

namespace Models
{
    [System.Serializable]
    public class TeamRecordsModel : BaseModel
    {
        [SerializeField] private List<TeamRecordModel> list = new List<TeamRecordModel>();
        public TeamRecordModel GetRecord(string key)
        {
            TeamRecordModel result = list.Find(x => x.Key.Equals(key));
            if (result == null)
            {
                result = new TeamRecordModel(key);
                list.Add(result);
            }
            return result;
        }
        public void SetNewTeam(string key, TeamFightData value) { GetRecord(key).SetNewTeam(value); }
    }
}
