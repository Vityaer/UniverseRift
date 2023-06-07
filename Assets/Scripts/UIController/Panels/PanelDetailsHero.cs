using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using Assets.Scripts.Models.Heroes;
using Assets.Scripts.City.TrainCamp;
using UIController.ControllerPanels;

public class PanelDetailsHero : BasePanelScript{
	public TextMeshProUGUI textAttackSkill, textDeffendSkill, textSpeed, textCountCounterAttack, textCanRetaliation;
	private HeroModel hero;
	protected override void OnOpen(){
		hero = TrainCamp.Instance.ReturnSelectHero();
		textAttackSkill.text = hero.Characts.baseCharacteristic.Attack.ToString();
		textDeffendSkill.text = hero.Characts.baseCharacteristic.Defense.ToString();
		textSpeed.text = hero.Characts.baseCharacteristic.Speed.ToString();
		textCountCounterAttack.text = hero.Characts.baseCharacteristic.CountCouterAttack.ToString();
		textCanRetaliation.text = hero.Characts.baseCharacteristic.CanRetaliation ? "Yes" : "No";
	}
}
