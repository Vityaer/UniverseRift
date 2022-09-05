using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;
using DG.Tweening;
public partial class HeroControllerScript : MonoBehaviour{
	//Register
		public delegate void Del();
		public delegate void DelFloat(float damage);
		public delegate void DelListTarget(List<HeroControllerScript> listTarget);
		private Del delsOnStartFight;
		private DelFloat delsOnStrikeFinish;
		private Del delsOnTakingDamage;
		private Del delsOnDeathHero;
		private Del delsOnHPLess50;
		private Del delsOnHPLess30;
		private Del delsOnHeal;
		private DelListTarget delsOnStrike;
		private DelListTarget delsOnSpell;
		private DelListTarget delsOnListSpell;
		public void RegisterOnStartFight(Del d){delsOnStartFight += d;}
		public void RegisterOnStrike(DelListTarget d){delsOnStrike += d;}
		public void RegisterOnTakingDamage(Del d){delsOnTakingDamage += d;}
		public void RegisterOnDeathHero(Del d){delsOnDeathHero += d;}
		public void RegisterOnHeal(Del d){delsOnHeal += d;}
		public void RegisterOnStrikeFinish(DelFloat d){delsOnStrikeFinish += d;}
		public void RegisterOnHPLess50(Del d){delsOnHPLess50 += d;}
		public void RegisterOnHPLess30(Del d){delsOnHPLess30 += d;}
		public void RegisterOnSpell(DelListTarget d){delsOnSpell += d; }
		public void RegisterOnGetListForSpell(DelListTarget d){delsOnListSpell += d;}
	//Action event	
		protected void OnStartFight(){ if(delsOnStartFight != null) delsOnStartFight();}
		protected void OnStrike(){if(delsOnStrike != null) delsOnStrike(listTarget);}
		protected void OnTakingDamage(){if(delsOnTakingDamage != null) delsOnTakingDamage();}
		protected void OnDeathHero(){if(delsOnDeathHero != null) delsOnDeathHero();}
		protected void OnHeal(){if(delsOnHeal != null) delsOnHeal();}
		public void OnHPLess50(){if(delsOnHPLess50 != null) delsOnHPLess50();}
		public void OnHPLess30(){if(delsOnHPLess30 != null) delsOnHPLess30();}
		protected void OnFinishStrike(){if(delsOnStrikeFinish != null) delsOnStrikeFinish(damageFromStrike);}
		protected void OnSpell(){if(delsOnSpell != null) delsOnSpell(listTarget);}


	private static Action<HeroControllerScript> observerStartAction;
		public static void RegisterOnStartAction(Action<HeroControllerScript> d){ observerStartAction += d;}
		public static void UnregisterOnStartAction(Action<HeroControllerScript> d){ observerStartAction -= d;}
		protected void OnStartAction(){if(observerStartAction != null) observerStartAction(this);}

		private static Action observerEndAction;
		public static void RegisterOnEndAction(Action d){ observerEndAction += d;}
		public static void UnregisterOnEndAction(Action d){ observerEndAction -= d;}
		protected void OnEndAction(){if(observerEndAction != null) observerEndAction();}

		private static Action observerEndSelectCell;
		public static void RegisterOnEndSelectCell(Action d){ observerEndSelectCell += d;}
		public static void UnregisterOnEndSelectCell(Action d){ observerEndSelectCell -= d;}
		protected void OnEndSelectCell(){if(observerEndSelectCell != null) observerEndSelectCell();}

		
	protected void DeleteAllDelegate(){
			FightControllerScript.Instance.RemoveHeroWithActionAll(this);
			Del delsOnStartFight = null;
			Del delsOnStrike = null;
			Del delsOnTakingDamage = null;
			Del delsOnDeathHero = null;
			Del delsOnHPLess50 = null;
			Del delsOnHPLess30 = null;
			Del delsOnHeal = null;
			DelListTarget delsOnSpell = null;
			DelListTarget delsOnListSpell = null;
		}	
}