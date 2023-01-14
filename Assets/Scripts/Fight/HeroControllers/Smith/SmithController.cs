using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SmithController : HeroControllerScript{
	protected override void DoSpell(){
		statusState.ChangeStamina(-100);
		anim.Play("Spell");
	}
	private void AroundStun(){
		List<NeighbourCell> neighbours = myPlace.GetAvailableNeighbours;
		listTarget.Clear();
		for(int i = 0; i < neighbours.Count; i++){
			Debug.Log(neighbours[i].Cell.gameObject.name);
			if(neighbours[i].GetHero != null){
				listTarget.Add(neighbours[i].GetHero);
				// Debug.Log("find hero", neighbours[i].GetHero.gameObject);
			}
		}
		OnSpell(listTarget);
		EndTurn();
	}
}