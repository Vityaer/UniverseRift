using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Sirenix.OdinInspector;
using Sirenix.Serialization;
[CreateAssetMenu(fileName = "NewChapter", menuName = "Chapter/Campaign Chapter", order = 55)]
public class CampaignChapter : ScriptableObject{
	public string Name;
	public int numChapter; 
	[OdinSerialize] public List<CampaignMission> missions = new List<CampaignMission>();

}
