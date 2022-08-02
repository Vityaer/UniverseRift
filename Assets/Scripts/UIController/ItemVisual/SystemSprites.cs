using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Sirenix.OdinInspector;
using Sirenix.Serialization;
public  class SystemSprites : MonoBehaviour{
	public List<GeneralSprite> generalListSprites = new List<GeneralSprite>();
	public List<RaceSprite> raceListSprites = new List<RaceSprite>();
	public List<VocationSprite> vocationListSprites = new List<VocationSprite>();

	public void AddSprite(){

	}

	public  Sprite GetSprite(Race nameRace){ return raceListSprites.Find(x => x.race == nameRace)?.image;}
	public  Sprite GetSprite(Vocation nameVocation){return vocationListSprites.Find(x => x.vocation == nameVocation)?.image;}
	public Sprite GetSprite(SpriteName name){return generalListSprites.Find(x => x.name == name)?.image;}
	private static SystemSprites instance;
	public static SystemSprites Instance{get => instance;}
	void Awake(){instance = this;}
}	
[System.Serializable] public abstract class BaseItemSprite{ public Sprite image; }
[System.Serializable] public class RaceSprite : BaseItemSprite{ public Race race; }
[System.Serializable] public class VocationSprite : BaseItemSprite{ public Vocation vocation; }
[System.Serializable] public class GeneralSprite : BaseItemSprite{ public SpriteName name; }


public enum SpriteName{
	OneStarHero = 1,
	TwoStarHero = 2,
	ThreeStarHero = 3,
	FourStartHero = 4,
	FiveStarHero = 5,
	BaseSplinterHero = 6,
	BaseSplinterArtifact = 7
}