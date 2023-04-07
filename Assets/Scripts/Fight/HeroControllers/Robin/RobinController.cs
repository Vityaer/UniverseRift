using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RobinController : HeroControllerScript{

	private int _hitSpecialCount = 0;
	[Header("Spell Data")]
	public GameObject robinArrow;

	private void CreateRobinArrow()
	{
		FightControllerScript.Instance.ChooseEnemies(side, 3, listTarget);
		Debug.Log($"listTarget: {listTarget.Count}");
		foreach (HeroControllerScript target in listTarget)
		{
			Debug.Log("CreateRobinArrow");
			var arrow = Instantiate(robinArrow, tr.position, Quaternion.identity).GetComponent<RobinArrow>();
			arrow.SetTarget(target, new Strike(hero.characts.Damage, hero.characts.GeneralAttack, typeStrike: typeStrike, isMellee: false));
			arrow.RegisterOnCollision(OnSpecialHit);
		}
	}

	private void OnSpecialHit()
	{
		_hitSpecialCount++;
		if(_hitSpecialCount == listTarget.Count)
		{
			Debug.Log("OnSpecialHit finish");
			RemoveFightRecordActionMe();
			OnSpell(listTarget);
			EndTurn();
		}
	}

	protected override void DoSpell()
	{
		AddFightRecordActionMe();
		_hitSpecialCount = 0;
		statusState.ChangeStamina(-100);
		anim.Play("Spell");
	} 
}
