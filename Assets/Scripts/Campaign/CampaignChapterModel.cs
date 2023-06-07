using Models;
using Sirenix.Serialization;
using System.Collections.Generic;

public class CampaignChapterModel : BaseModel
{
    public string Name;
    public int numChapter;
    [OdinSerialize] public List<CampaignMission> Missions = new List<CampaignMission>();

}
