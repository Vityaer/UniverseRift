using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;
using DG.Tweening;
public partial class HeroControllerScript : MonoBehaviour{
	//Animations
	private const string ANIMATION_ATTACK = "Attack",
						 ANIMATION_GET_DAMAGE = "GetDamage",
						 ANIMATION_DEATH = "Death",
						 ANIMATION_MOVE = "Move",
						 ANIMATION_IDLE = "Idle",
						 ANIMATION_DISTANCE_ATTACK = "Shoot";
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
	private void DefaultAnimDistanceAttack(List<HeroControllerScript> enemies){
		AddFightRecordActionMe();
		GameObject arrow;
		hitCount = 0;
		this.listTarget = enemies;
		foreach (HeroControllerScript target in listTarget) {
			arrow = Instantiate(hero.PrefabArrow, tr.position, Quaternion.identity);
			arrow.GetComponent<ArrowScript>().SetTarget(target, new Strike(hero.characts.Damage, hero.characts.GeneralAttack, typeStrike: typeStrike, isMellee: false));
			arrow.GetComponent<ArrowScript>().RegisterOnCollision(HitCount);
		}
	}
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
	private void DefaultAnimDeath(){
		isDeath = true;
		AddFightRecordActionMe();
		if(sequenceAnimation != null) sequenceAnimation.Kill();
		sequenceAnimation = DOTween.Sequence()
			.Append(tr.DOScaleY(0f, 0.5f).OnComplete(Death));
	}
}