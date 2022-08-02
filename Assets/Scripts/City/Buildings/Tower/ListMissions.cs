using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "NewListMission", menuName = "Missions/List Mission", order = 56)]
public class ListMissions : ScriptableObject{
	public string Name;
	public List<Mission> missions = new List<Mission>();
	public int Count{get => missions.Count;}
}
