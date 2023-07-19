using Models;
using Sirenix.Serialization;
using System.Collections.Generic;

namespace Campaign
{
    public class CampaignChapterModel : BaseModel
    {
        public string Name;
        public int numChapter;
        public List<CampaignMissionModel> Missions = new List<CampaignMissionModel>();
    }
}