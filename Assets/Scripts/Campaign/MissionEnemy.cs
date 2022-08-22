using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class MissionEnemy{
	public HeroName heroName = HeroName.Peasant;
	private InfoHero _enemyPrefab = null;
	public InfoHero enemyPrefab{
		get{
			if(_enemyPrefab == null) _enemyPrefab = TavernScript.Instance.GetInfoHero((int) heroName);
			return _enemyPrefab;	
		}
	}
	
	public int level = 1;
}
public enum HeroName{
	Peasant = 1101,
	Militia = 1201,
	Raccoon = 2101,
	Pegasus = 2401,
	Spirit  = 3101,
	DeathKnight = 3501,
	Imp = 4101,
	Demoniac = 4102,
	templeGuard = 5301
}