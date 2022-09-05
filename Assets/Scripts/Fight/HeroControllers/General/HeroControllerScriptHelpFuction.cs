using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;
using DG.Tweening;
public partial class HeroControllerScript : MonoBehaviour{

	protected void PrepareOnStartTurn(){
		if(this.side == Side.Left){
			FightUI.Instance.OpenControllers(this);
		}else{
			FightUI.Instance.CloseControllers();
		}
		listTarget.Clear();
		FindAvailableCells();
		WaitingSelectTarget();
		OnStartAction();
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
	protected Side GetSideFace(){
		if(transform.localScale.x < 0){
			return Side.Left;
		}else{
			return Side.Right;
		}
	} 
	protected void FlipX(){
		Vector3 locScale = transform.localScale;
		locScale.x *= -1;
		transform.localScale = locScale; 
	} 
//Fight helps
	protected bool CanAttackHero(HeroControllerScript otherHero){
		if(this.side != otherHero.side){
			return true;
		}else{
			return false;
		}
	}
	protected void RefreshOnEndRound(){
		currentCountCounterAttack = hero.GetBaseCharacteristic.CountCouterAttack;
	}
 	protected void ShakeCamera(){ CameraShake.Shake(0.8f, 2f, CameraShake.ShakeMode.XY); }

	protected void IsSide(Side side){
		this.side = side;
		delta = (side == Side.Left) ? new Vector2(-0.6f, 0f) : new Vector2(0.6f, 0f);
		if(side == Side.Right) FlipX();
	}
	protected bool CanCounterAttack(Strike strike, HeroControllerScript heroForCounterAttack, HeroControllerScript heroWasAttack){
		bool result = false;
		if(strike.isMellee && (heroForCounterAttack != heroWasAttack) && heroWasAttack.CanRetaliation){
			if(currentCountCounterAttack > 0){
				result = true;
			}
		}
		return result;
	}
}