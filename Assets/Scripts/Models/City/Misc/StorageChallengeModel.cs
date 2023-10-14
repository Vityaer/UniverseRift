using Models.Fights.Campaign;
using System;
using System.Collections.Generic;

namespace Models.City.Misc
{
    [Serializable]
    public class StorageChallengeModel : BaseModel
    {
        public List<MissionModel> Missions = new List<MissionModel>();

    }
}
