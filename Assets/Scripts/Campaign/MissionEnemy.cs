using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class MissionEnemy{
	public InfoHero enemyPrefab;
	public int level = 1;
	public int rating = 1;
	public int position = 1;
	public int HP = 0;
	private int currentHP = 0;
	public int CurrentHP{get{ return currentHP;} set{ Debug.Log("change hp on" + value.ToString()); currentHP = value;}}
	public int GetHP{
		get{
			if((HP > 0) && (currentHP < HP)){
				return currentHP;
			}else{
				return 0;
			}
		}
	}
	public bool isAlive{get => (HP >= 0);}
}
