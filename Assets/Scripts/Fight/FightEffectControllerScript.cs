using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FightEffectControllerScript : MonoBehaviour{
  
	[Header("Simple Effects")]
	public GameObject healPrefab;
	private List<GameObject> healGameObjects;
	[Header("Constant Effects")]
	public Color statePetrification;
	public Color stateFreezing;
	public Color stateAstral;
	public Color stateClear;
	public GameObject stateStun;
	public GameObject stateSilence;

	[Header("Dots")]
	public GameObject dotPoisonPrefab;
	public GameObject dotBleendingPrefab;
	public GameObject dotRotPrefab;
	public GameObject dotCorrosionPrefab;
	public GameObject dotCombustionPrefab;
//API
	public void CreateHealObject(Transform parent){
		Instantiate(healPrefab, parent.position, Quaternion.identity, parent);
	}
	public void CreateDot(Transform parent, TypeDot dot){
		switch (dot) {
			case TypeDot.Poison:
				Instantiate(dotPoisonPrefab, parent.position, Quaternion.identity, parent);
				break;
			case TypeDot.Bleending:
				Instantiate(dotBleendingPrefab, parent.position, Quaternion.identity, parent);
				break;
			case TypeDot.Rot:
				Instantiate(dotRotPrefab, parent.position, Quaternion.identity, parent);
				break;
			case TypeDot.Corrosion:
				Instantiate(dotCorrosionPrefab, parent.position, Quaternion.identity, parent);
				break;
			case TypeDot.Combustion:
				Instantiate(dotCombustionPrefab, parent.position, Quaternion.identity, parent);
				break;			
		}
	}

	public void CastEffectStateOnHero(GameObject hero, State state){
		switch (state) {
			case State.Stun:
				Instantiate(stateStun, hero.transform.position, Quaternion.identity, hero.transform);
				break;
			case State.Astral:
				hero.GetComponent<SpriteRenderer>().color = stateAstral;
				break;
			case State.Freezing:
				hero.GetComponent<SpriteRenderer>().color = stateFreezing;
				break;
			case State.Silence:	
				Instantiate(stateStun, hero.transform.position, Quaternion.identity, hero.transform);
				break;
			case State.Petrification:
				hero.GetComponent<SpriteRenderer>().color = statePetrification;
				break;
										
		}
	}
	public void ClearEffectStateOnHero(GameObject hero, State state){
		hero.GetComponent<SpriteRenderer>().color = stateClear;
		switch (state){
			case State.Silence:
				Destroy(hero.transform.Find("SilencePrefab(Clone)").gameObject);
				break;
			case State.Stun:
				Destroy(hero.transform.Find("StunPrefab(Clone)").gameObject);
				break;
		}
	}
//About me	
	private static FightEffectControllerScript instance;
	public static FightEffectControllerScript Instance {get => instance;}
	void Awake(){
		instance = this;
	}
}
