using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BossControllerScript : MonoBehaviour{

	public CampaignChapter guildBosses;
	public int currentNumBoss = 0;
	public Mission mission;
	void Start(){
		PrepareNewMission();
	}
	void PrepareNewMission(){
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