using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HeroFightStatistics : MonoBehaviour{
	public  List<HeroControllerScript> leftTeam  = new List<HeroControllerScript>(); 
	public  List<HeroControllerScript> rightTeam = new List<HeroControllerScript>(); 

}
public class HeroStatistic{

	private HeroControllerScript hero;
	public HeroControllerScript Hero{get => hero;}

	private float damageDone;
	public float DamageDone{get => damageDone;}

	private float healDone;
	public float HealDone{get => healDone;}

	public HeroStatistic(HeroControllerScript hero){
		this.hero = hero;
	}
}