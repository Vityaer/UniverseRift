using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
public class PanelDetailsHero : BasePanelScript{
	public TextMeshProUGUI textAttackSkill, textDeffendSkill, textSpeed, textCountCounterAttack, textCanRetaliation;
	private InfoHero hero;
	protected override void OnOpen(){
		hero = TrainCampScript.Instance.ReturnSelectHero();
		textAttackSkill.text = hero.characts.baseCharacteristic.Attack.ToString();
		textDeffendSkill.text = hero.characts.baseCharacteristic.Defense.ToString();
		textSpeed.text = hero.characts.baseCharacteristic.Speed.ToString();
		textCountCounterAttack.text = hero.characts.baseCharacteristic.CountCouterAttack.ToString();
		textCanRetaliation.text = hero.characts.baseCharacteristic.CanRetaliation ? "Yes" : "No";
	}
}
