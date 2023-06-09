using Models.Fights.Campaign;
using System;

namespace Campaign
{
    public class BossMission : MissionModel, ICloneable
    {

        public void SaveResult()
        {
            // Debug.Log("change hp");
            // 		int id, currentHP;
            // 		for(int j = 0; j < mission.listEnemy.Count; j++){
            // 			Debug.Log(mission.listEnemy[j].enemyPrefab.name);
            // 			id = int.Parse(mission.listEnemy[j].enemyPrefab.name);
            // 			currentHP = 0;
            // 			for(int i=0; i < rightTeam.Count; i++){
            // 				if(rightTeam[i].heroController != null){
            // 					Debug.Log(rightTeam[i].heroController.hero.generalInfo.idHero.ToString() + " and " + id.ToString());
            // 					if(rightTeam[i].heroController.hero.generalInfo.idHero == id){
            // 						currentHP = rightTeam[i].heroController.hero.characts.HP;
            // 						break;
            // 					}	
            // 				}
            // 			}
            // 			mission.listEnemy[j].CurrentHP = currentHP; 
            // 		}
        }
        public object Clone()
        {
            return new BossMission();
        }
    }
}