using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;
using DG.Tweening;
public class HeroControllerScript : MonoBehaviour{
	public HeroStatusScript statusState;
	private Transform tr;
	[SerializeField] private HexagonCellScript myPlace;
	public List<HeroControllerScript> listTarget = new List<HeroControllerScript>();
	private Animator anim;
	public float speedMove = 2f;
	public float speedAnimation = 1f;
	private Rigidbody2D rb;
	public Hero hero;
	public Side side = Side.Left;
	private Vector3 delta = new Vector2(-0.6f, 0f);
	public int maxCountCounterAttack = 1;
	private int currentCountCounterAttack = 1;
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
	void Awake(){
		statusState = GetComponent<HeroStatusScript>();
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
		AddFightRecordActionMe();
		if((isDeath == false) && statusState.GetCanAction()){
			PrepareOnStartTurn();
		}else{
			EndTurn();
		}
	}
	protected void PrepareOnStartTurn(){
		listTarget.Clear();
		FindAvailableCells();
		WaitingSelectTarget();
		OnStartAction();
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
 	private void StartMelleeAttackOtherHero(HexagonCellScript targetCell, HeroControllerScript enemy){
        coroutineAttackEnemy = null;
		coroutineAttackEnemy = StartCoroutine(IMelleeAttackOtherHero(targetCell, enemy != null ? () => {StartAttack(enemy);} : null));
 	}
 	public void StartDistanceAttackOtherHero(HeroControllerScript enemy){
		HexagonCellScript.UnregisterOnClick(SelectHexagonCell);
		OnEndSelectCell();
 		CreateArrow(new List<HeroControllerScript>(){enemy});
 	}
 	private bool onGround = true;
 	Stack<HexagonCellScript> way = new Stack<HexagonCellScript>();
	IEnumerator IMelleeAttackOtherHero(HexagonCellScript targetCell, Action actionOnEndMove){
		HexagonCellScript.UnregisterOnClick(SelectHexagonCell);
		OnEndSelectCell();
		Debug.Log("find way...");
		way = HexagonGridScript.Instance.FindWay(myPlace, targetCell, typeMovement: hero.GetBaseCharacteristic.typeMovement);
		Vector3 targetPos, startPos;
		float t = 0f;
		HexagonCellScript currentCell;
		Debug.Log("start move///");
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
        EndTurn();
 	} 

	private void StartAttack(HeroControllerScript targetForAttack){
		statusState.ChangeStamina(30f);
		PlayAnimation(ANIMATION_ATTACK, () => DefaultAnimAttack(targetForAttack));
	}
	private void CouterAttack(Strike strike){
		if(CanCounterAttack(strike, this, FightControllerScript.Instance.GetCurrentHero())){
			PlayAnimation(ANIMATION_ATTACK, () => DefaultAnimAttack(FightControllerScript.Instance.GetCurrentHero()));
			currentCountCounterAttack -= 1;
		}
	}
	private bool CanCounterAttack(Strike strike, HeroControllerScript heroForCounterAttack, HeroControllerScript heroWasAttack){
		bool result = false;
		if(strike.isMellee && (heroForCounterAttack != heroWasAttack) && heroWasAttack.CanRetaliation){
			if(currentCountCounterAttack > 0){
				result = true;
			}
		}
		return result;
	}
	void CreateArrow(List<HeroControllerScript> listTarget){
		GameObject arrow;
		hitCount = 0;
		this.listTarget = listTarget;
		foreach (HeroControllerScript target in listTarget) {
			arrow = Instantiate(Resources.Load<GameObject>("CreateObjects/Bullet"), tr.position, Quaternion.identity);
			arrow.GetComponent<ArrowScript>().SetTarget(target, new Strike(hero.characts.Damage, hero.characts.GeneralAttack, typeStrike: typeStrike, isMellee: false));
			arrow.GetComponent<ArrowScript>().RegisterOnCollision(HitCount);
		}
	}
	public int hitCount = 0;
	public void HitCount(){
		Debug.Log("distance hit");
		OnStrike();
		hitCount += 1;
		if(hitCount == listTarget.Count) { 
			EndTurn();
		}
	}
	protected virtual void DoSpell(){
		anim.Play("SpecialAttack");
		OnSpell();
		Debug.Log("do spell");
		EndTurn();
	} 	
//Brain hero 
	public void StartDefend(){
		Debug.Log("defend");
		EndTurn();
	}	
 	protected virtual void ChooseEnemies(List<HeroControllerScript> listTarget, int countTarget){
 		listTarget.Clear();
 		if(countTarget == 0){
 			countTarget = 1;
 			hero.characts.CountTargetForSimpleAttack = 1;
 		}
 		FightControllerScript.Instance.ChooseEnemies(side, countTarget, listTarget);

 	}
 	private StageAction ChooseAction(){
 		if(Mellee){
	 		return StageAction.MoveToTarget;
 		}else{
	 		return StageAction.Attack;
 		}
 	}
//End action 	
	public void EndTurn(){
		OnEndAction();
		PlayAnimation(ANIMATION_IDLE, DefaultAnimIdle);
		RemoveFightRecordActionMe();
	}
	
//API
	public void IsSide(Side side){
		this.side = side;
		delta = (side == Side.Left) ? new Vector2(-0.6f, 0f) : new Vector2(0.6f, 0f);
		if(side == Side.Right) FlipX();
	}
	public void SetHero(InfoHero infoHero, HexagonCellScript place, Side side){
		myPlace = place;
		place.SetHero(this);
		IsSide(side);
		this.hero.SetHero(infoHero);
		statusState.SetMaxHealth(this.hero.characts.HP);
	}
	public Vector3 GetPosition(){
		return tr.position;
	}
	public void GetDamage(Strike strike){
		if(statusState.PermissionGetStrike(strike)){
			OnTakingDamage();
			hero.GetDamage(strike);
			statusState.ChangeHP(hero.characts.HP);
			statusState.ChangeStamina(10f);
			if(hero.characts.HP > 0){
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
		Debug.Log("delete hero");
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
		statusState.Death();
		FightControllerScript.Instance.DeleteHero(this);
		isDeath = true;
		DeleteHero();
	}
	private float damageFromStrike;
	public void MessageDamageAfterStrike(float damage){
		damageFromStrike += damage;
	}
//Animations
	private const string ANIMATION_ATTACK = "Attack",
						 ANIMATION_GET_DAMAGE = "GetDamage",
						 ANIMATION_DEATH = "Death",
						 ANIMATION_MOVE = "Move",
						 ANIMATION_IDLE = "Idle";
	private void PlayAnimation(string nameAnimation, Action defaultAnimation = null){
		if(CheckExistAnimation(nameAnimation)){
			anim.Play(nameAnimation);
		}else{
			if(defaultAnimation != null)
				defaultAnimation();
		}
	}
	Dictionary<string, bool> animationsExist = new Dictionary<string, bool>(); 
	private bool CheckExistAnimation(string nameAnimation){
		bool result = false;
		if(animationsExist.ContainsKey(nameAnimation)){
			result = animationsExist[nameAnimation]; 
		}else{
			int stateId = Animator.StringToHash(nameAnimation);
			bool animExist = anim.HasState(0, stateId);
			animationsExist.Add(nameAnimation, animExist);
			result = animExist;
		}
		return result;
	}
	Tween sequenceAnimation;
	private void DefaultAnimAttack(HeroControllerScript enemy){
		AddFightRecordActionMe();
		if(sequenceAnimation != null) sequenceAnimation.Kill();
		bool needFlip = NeedFlip(enemy);
		Vector3 rotateAttack = Vector3.zero;
		if(needFlip){ FlipX(); }
		rotateAttack = new Vector3(0, 0, GetSideFace() == Side.Left ? -45 : 45);
		sequenceAnimation = DOTween.Sequence()
					.Append( tr.DORotate(rotateAttack, 0.25f))
					.Append(tr.DORotate(Vector3.zero, 0.25f).OnComplete(() => {GiveDamage(enemy); RemoveFightRecordActionMe(); if(needFlip) FlipX();} ));
	}
	private bool NeedFlip(HeroControllerScript enemy){
		bool result = false;
		NeighbourDirection direction = NeighbourCell.GetDirection(myPlace, enemy.Cell);
		switch(direction){
			case NeighbourDirection.UpLeft:
			case NeighbourDirection.Left:
			case NeighbourDirection.BottomLeft:
				result = (side != Side.Right);
				break;
			case NeighbourDirection.UpRight:
			case NeighbourDirection.Right:
			case NeighbourDirection.BottomRight:
				result = (side != Side.Left);
				break;	
		} 
		return result;
	}
	public void GiveDamage(HeroControllerScript enemy){
		enemy.GetDamage(new Strike(hero.characts.Damage, hero.characts.GeneralAttack, typeStrike: typeStrike));
	} 
	private void DefaultAnimDeath(){
		if(sequenceAnimation != null) sequenceAnimation.Kill();
		sequenceAnimation = DOTween.Sequence()
			.Append(tr.DOScaleY(0f, 0.5f).OnComplete(Death));
	}
	private void DefaultAnimIdle(){

	}	
	private void DefaultAnimMove(){

	}
	private void DefaultAnimGetDamage(Strike strike){
		AddFightRecordActionMe();
		if(sequenceAnimation != null) sequenceAnimation.Kill();
		Vector3 rotateGiveDamage = new Vector3(0, 0, -45);
		sequenceAnimation = DOTween.Sequence().Append( tr.DORotate(rotateGiveDamage, 0.25f) )
				.Append(tr.DORotate(Vector3.zero, 0.25f).OnComplete(() => {CouterAttack(strike); RemoveFightRecordActionMe();} ));
	}
//Event
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
		private void OnStartFight(){ if(delsOnStartFight != null) delsOnStartFight();}
		private void OnStrike(){if(delsOnStrike != null) delsOnStrike(listTarget);}
		private void OnTakingDamage(){if(delsOnTakingDamage != null) delsOnTakingDamage();}
		private void OnDeathHero(){if(delsOnDeathHero != null) delsOnDeathHero();}
		private void OnHeal(){if(delsOnHeal != null) delsOnHeal();}
		public void OnHPLess50(){if(delsOnHPLess50 != null) delsOnHPLess50();}
		public void OnHPLess30(){if(delsOnHPLess30 != null) delsOnHPLess30();}
		public void OnFinishStrike(){if(delsOnStrikeFinish != null) delsOnStrikeFinish(damageFromStrike);}
		public void OnSpell(){if(delsOnSpell != null) delsOnSpell(listTarget);}

		public void GetListForSpell(List<HeroControllerScript> listTarget){
			statusState.ChangeStamina(-100);
			if(delsOnListSpell != null)
				delsOnListSpell(listTarget);
		}
		private void DeleteAllDelegate(){
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

		private static Action<HeroControllerScript> observerStartAction;
		public static void RegisterOnStartAction(Action<HeroControllerScript> d){ observerStartAction += d;}
		public static void UnregisterOnStartAction(Action<HeroControllerScript> d){ observerStartAction -= d;}
		private void OnStartAction(){if(observerStartAction != null) observerStartAction(this);}

		private static Action observerEndAction;
		public static void RegisterOnEndAction(Action d){ observerEndAction += d;}
		public static void UnregisterOnEndAction(Action d){ observerEndAction -= d;}
		private void OnEndAction(){if(observerEndAction != null) observerEndAction();}

		private static Action observerEndSelectCell;
		public static void RegisterOnEndSelectCell(Action d){ observerEndSelectCell += d;}
		public static void UnregisterOnEndSelectCell(Action d){ observerEndSelectCell -= d;}
		private void OnEndSelectCell(){if(observerEndSelectCell != null) observerEndSelectCell();}

	private Side GetSideFace(){
		if(transform.localScale.x < 0){
			return Side.Left;
		}else{
			return Side.Right;
		}
	} 
	private void FlipX(){
		Vector3 locScale = transform.localScale;
		locScale.x *= -1;
		transform.localScale = locScale; 
	} 
//Fight helps
	private bool CanAttackHero(HeroControllerScript otherHero){
		if(this.side != otherHero.side){
			return true;
		}else{
			return false;
		}
	}
	private void RefreshOnEndRound(){
		currentCountCounterAttack = hero.GetBaseCharacteristic.CountCouterAttack;
	}
	private void AddFightRecordActionMe(){ FightControllerScript.Instance.AddHeroWithAction(this); }
	private void RemoveFightRecordActionMe(){ FightControllerScript.Instance.RemoveHeroWithAction(this); }
}





public enum Side{
	Left,
	Right,
	All
}