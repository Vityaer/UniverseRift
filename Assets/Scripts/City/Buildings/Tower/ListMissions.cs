using Models.Fights.Campaign;
using System.Collections.Generic;
using UnityEngine;

namespace City.Buildings.Tower
{
    [CreateAssetMenu(fileName = "NewListMission", menuName = "Missions/List Mission", order = 56)]
    public class ListMissions : ScriptableObject
    {
        public string Name;
        public List<MissionModel> missions = new List<MissionModel>();
        public int Count { get => missions.Count; }
    }
}