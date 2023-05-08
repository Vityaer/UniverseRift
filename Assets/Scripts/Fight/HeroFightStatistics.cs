using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HeroFightStatistics : MonoBehaviour{
	public  List<HeroController> leftTeam  = new List<HeroController>(); 
	public  List<HeroController> rightTeam = new List<HeroController>(); 

}
public class HeroStatistic{

	private HeroController hero;
	public HeroController Hero{get => hero;}

	private float damageDone;
	public float DamageDone{get => damageDone;}

	private float healDone;
	public float HealDone{get => healDone;}

	public HeroStatistic(HeroController hero){
		this.hero = hero;
	}
}