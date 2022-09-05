using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RobinController : HeroControllerScript{

	private int hitSpecialCount = 0;
	[Header("Spell Data")]
	public GameObject robinArrow;
	private void CreateRobinArrow(){
		AddFightRecordActionMe();
		GameObject arrow;
		FightControllerScript.Instance.ChooseEnemies(side, 1, listTarget);
		foreach (HeroControllerScript target in listTarget) {
			arrow = Instantiate(robinArrow, tr.position, Quaternion.identity);
			arrow.GetComponent<RobinArrow>().SetTarget(target);
			arrow.GetComponent<RobinArrow>().RegisterOnCollision(OnSpecialHit);
		}
	}
	private void OnSpecialHit(){
		hitSpecialCount++;
		if(hitSpecialCount == listTarget.Count){
			OnSpell();
			EndTurn();
		}
	}
	protected override void DoSpell(){
		hitSpecialCount = 0;
		statusState.ChangeStamina(-100f);
		anim.Play("Spell");
	} 
}
