using Models.Fights.Campaign;
using UnityEngine;

namespace City.Buildings.Guild.BossRaid
{
    public class BossController : MonoBehaviour
    {
        public CampaignChapterModel CampaignChapterModel;
        public int CurrentNumBoss;
        public MissionModel Mission;

        void Start()
        {
            PrepareNewMission();
        }

        void PrepareNewMission()
        {
            // mission = (Mission) guildBosses.missions[currentNumBoss].Clone();
            // foreach(MissionEnemy enemy in mission.listEnemy){
            // 	enemy.CurrentHP = enemy.HP;
            // }
        }
        // public MissionEnemy GetBoss(){
        // 	if(mission.listEnemy.Count == 0){
        // 		PrepareNewMission();
        // 	}
        // 	return mission.listEnemy[0];
        // }
    }
}