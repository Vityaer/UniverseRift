using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;
using DG.Tweening;
public partial class HeroControllerScript : MonoBehaviour{
	[Header("Components")]
	public HeroStatusScript statusState;
	protected Transform tr;
	[SerializeField] protected HexagonCellScript myPlace;
	[HideInInspector] public List<HeroControllerScript> listTarget = new List<HeroControllerScript>();
	protected Rigidbody2D rb;
	protected Animator anim;
	[Header("Characteristics")]
	public float speedMove = 2f;
	public float speedAnimation = 1f;
	public Hero hero;
	public Side side = Side.Left;
	protected Vector3 delta = new Vector2(-0.6f, 0f);
	public int maxCountCounterAttack = 1;
	protected int currentCountCounterAttack = 1;
	public enum StageAction{
		MoveToTarget,
		Attack,
		MoveHome,
		Spell
	}
	public StageAction action;
	public bool CanRetaliation{get => hero.characts.baseCharacteristic.CanRetaliation;}
	public HexagonCellScript Cell{get => myPlace;}
	public bool Mellee{get => hero.characts.baseCharacteristic.Mellee;}
	public TypeStrike typeStrike{get => hero.characts.baseCharacteristic.typeStrike;}
	public float Stamina{get => statusState.Stamina;}
	void Awake(){
		statusState = GetComponent<HeroStatusScript>();
		hero.statusState = statusState;
		tr          = GetComponent<Transform>();
		rb          = GetComponent<Rigidbody2D>();
        anim        = GetComponent<Animator>();
	}
	void Start(){
		hero.PrepareSkills(this);
		OnStartFight();
		FightControllerScript.Instance.RegisterOnEndRound(RefreshOnEndRound);
	}

	public void DoTurn(){
		Debug.Log("start turn: " + gameObject.name, gameObject);
		AddFightRecordActionMe();
		if((isDeath == false) && statusState.GetCanAction()){
			PrepareOnStartTurn();
		}else{
			EndTurn();
		}
	}
	protected void EndTurn(){
		ClearAction();
		OnEndAction();
		PlayAnimation(ANIMATION_IDLE, DefaultAnimIdle);
		FightControllerScript.Instance.RemoveHeroWithActionAll(this);
	}


	protected virtual void FindAvailableCells(){myPlace.StartCheckMove(hero.characts.baseCharacteristic.Speed, playerCanController: !AIController.Instance.CheckMeOnSubmission(side));}
	protected virtual void WaitingSelectTarget(){ HexagonCellScript.RegisterOnClick(SelectHexagonCell); }
	HexagonCellScript cellTarget;
	HeroControllerScript selectHero;
	public void SelectHexagonCell(HexagonCellScript cell){
		if(cell.CanStand){
			StartMelleeAttackOtherHero(cell, null);
		}else{
			if(cell.Hero != null){
				if(CanAttackHero(cell.Hero) ){
					selectHero = cell.Hero;
					HexagonCellScript.UnregisterOnClick(SelectHexagonCell);
					if((hero.characts.baseCharacteristic.Mellee == true)){
						if(cell.GetCanAttackCell){
							cell.RegisterOnSelectDirection(SelectDirectionAttack);
						}else{
							Debug.Log("i can't attak him");
						}
					}else{
						Debug.Log("i can shoot in him");
						StartDistanceAttackOtherHero(selectHero);
					}
				}	
			}
		}
	}
	public void SelectDirectionAttack(HexagonCellScript targetCell){
		StartMelleeAttackOtherHero(targetCell, selectHero);
	}
	public void SelectDirectionAttack(HexagonCellScript targetCell, HeroControllerScript otherHero){
		StartMelleeAttackOtherHero(targetCell, otherHero);
	}
//Attack
	Coroutine coroutineAttackEnemy;
 	protected void StartMelleeAttackOtherHero(HexagonCellScript targetCell, HeroControllerScript enemy){
        coroutineAttackEnemy = null;
		coroutineAttackEnemy = StartCoroutine(IMelleeAttackOtherHero(targetCell, enemy != null ? () => {StartAttack(enemy);} : null));
 	}
 	public void StartDistanceAttackOtherHero(HeroControllerScript enemy){
		HexagonCellScript.UnregisterOnClick(SelectHexagonCell);
		OnEndSelectCell();
		statusState.ChangeStamina(20f);
		PlayAnimation(ANIMATION_DISTANCE_ATTACK, () => DefaultAnimDistanceAttack(new List<HeroControllerScript>(){enemy}));
 	}
 	protected bool onGround = true;
 	Stack<HexagonCellScript> way = new Stack<HexagonCellScript>();
	IEnumerator IMelleeAttackOtherHero(HexagonCellScript targetCell, Action actionOnEndMove){
		Debug.Log("start move " + gameObject.name, gameObject);
		HexagonCellScript.UnregisterOnClick(SelectHexagonCell);
		OnEndSelectCell();
		way = HexagonGridScript.Instance.FindWay(myPlace, targetCell, typeMovement: hero.GetBaseCharacteristic.typeMovement);
		Vector3 targetPos, startPos;
		float t = 0f;
		HexagonCellScript currentCell;
		while(way.Count > 0){
			myPlace.ClearSublject();
			currentCell = way.Pop();
			startPos = tr.position;
			targetPos = currentCell.Position;
			t = 0f;
	        while(t <= 1f){
	        	t += Time.deltaTime * speedMove;
	        	tr.position = Vector2.Lerp(startPos, targetPos, t);
				yield return null;
	        }
	        tr.position = targetPos;
	        myPlace = currentCell;
	        myPlace.SetHero(this);
		}
		if(actionOnEndMove != null){
			yield return new WaitForSeconds(0.5f);
			actionOnEndMove();
		}
		Debug.Log("end turn " + gameObject.name);
        EndTurn();
 	} 

	protected void StartAttack(HeroControllerScript targetForAttack){
		statusState.ChangeStamina(30f);
		PlayAnimation(ANIMATION_ATTACK, () => DefaultAnimAttack(targetForAttack));
	}
	protected void CouterAttack(Strike strike){
		statusState.ChangeStamina(15f);
		if(CanCounterAttack(strike, this, FightControllerScript.Instance.GetCurrentHero())){
			PlayAnimation(ANIMATION_ATTACK, () => DefaultAnimAttack(FightControllerScript.Instance.GetCurrentHero()));
			currentCountCounterAttack -= 1;
		}
	}

	protected int hitCount = 0;
	protected void HitCount(){
		Debug.Log("distance hit");
		OnStrike();
		hitCount += 1;
		if(hitCount == listTarget.Count) { 
			RemoveFightRecordActionMe();
			EndTurn();
		}
	}
	protected virtual void DoSpell(){
		statusState.ChangeStamina(-100f);
		anim.Play("Spell");
		OnSpell();
		EndTurn();
	} 	
//Brain hero 
	
 	protected virtual void ChooseEnemies(List<HeroControllerScript> listTarget, int countTarget){
 		listTarget.Clear();
 		if(countTarget == 0){
 			countTarget = 1;
 			hero.characts.CountTargetForSimpleAttack = 1;
 		}
 		FightControllerScript.Instance.ChooseEnemies(side, countTarget, listTarget);

 	}
 	protected StageAction ChooseAction(){
 		if(Mellee){
	 		return StageAction.MoveToTarget;
 		}else{
	 		return StageAction.Attack;
 		}
 	}
//End action 	

 	protected void ClearAction(){
 		HexagonCellScript.UnregisterOnClick(SelectHexagonCell);
 		OnEndSelectCell();
 	}
	
//API


	public void SetHero(InfoHero infoHero, HexagonCellScript place, Side side){
		myPlace = place;
		place.SetHero(this);
		IsSide(side);
		this.hero.SetHero(infoHero);
		statusState.SetMaxHealth(this.hero.characts.HP);
	}
	public Vector3 GetPosition{get => tr.position;}
	public void GetDamage(Strike strike){
		if(statusState.PermissionGetStrike(strike)){
			OnTakingDamage();
			hero.GetDamage(strike);
			statusState.ChangeHP(hero.characts.HP);
			statusState.ChangeStamina(10f);
			if(hero.characts.HP > 0.1f){
				PlayAnimation(ANIMATION_GET_DAMAGE, () => DefaultAnimGetDamage(strike));
			}else{
				PlayAnimation(ANIMATION_DEATH, DefaultAnimDeath);
			}	
		}
	}

	public void GetHeal(float heal, TypeNumber typeNumber = TypeNumber.Num){
		hero.GetHeal(heal, typeNumber);
		statusState.ChangeHP(hero.characts.HP);
		FightEffectControllerScript.Instance.CreateHealObject(tr);
		OnHeal();
	}
	
	public void ChangeMaxHP(int amountChange, TypeNumber typeNumber = TypeNumber.Num){
		hero.ChangeMaxHP(amountChange, typeNumber);
		statusState.ChangeMaxHP(amountChange);
	}
	public void DeleteHero(){
		myPlace.ClearSublject();
		if(coroutineAttackEnemy != null) StopCoroutine(coroutineAttackEnemy);
		if(sequenceAnimation != null) sequenceAnimation.Kill();
		FightControllerScript.Instance.UnregisterOnEndRound(RefreshOnEndRound);
		DeleteAllDelegate();
		Destroy(gameObject);
	}	
	public void ClickOnMe(){
		myPlace?.ClickOnMe();
	}
	private bool isDeath = false;
	public bool IsDeath{get => isDeath;}
	private void Death(){
		RemoveFightRecordActionMe();
		statusState.Death();
		ClearAction();
		FightControllerScript.Instance.DeleteHero(this);
		DeleteHero();
	}
	private float damageFromStrike;
	public void MessageDamageAfterStrike(float damage){
		damageFromStrike += damage;
	}

	
	
	public void GiveDamage(HeroControllerScript enemy){
		enemy.GetDamage(new Strike(hero.characts.Damage, hero.characts.GeneralAttack, typeStrike: typeStrike));
	} 
//Event
		public void GetListForSpell(List<HeroControllerScript> listTarget){
			statusState.ChangeStamina(-100);
			if(delsOnListSpell != null)
				delsOnListSpell(listTarget);
		}
		

		


	
	protected void AddFightRecordActionMe(){ FightControllerScript.Instance.AddHeroWithAction(this); }
	protected void RemoveFightRecordActionMe(){ FightControllerScript.Instance.RemoveHeroWithAction(this); }
}





public enum Side{
	Left,
	Right,
	All
}