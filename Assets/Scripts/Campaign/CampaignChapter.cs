using Sirenix.Serialization;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "NewChapter", menuName = "Chapter/Campaign Chapter", order = 55)]
public class CampaignChapter : ScriptableObject
{
    public string Name;
    public int numChapter;
    [OdinSerialize] public List<CampaignMission> missions = new List<CampaignMission>();

}
