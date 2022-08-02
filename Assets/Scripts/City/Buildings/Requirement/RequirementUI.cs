﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using System;
using ObjectSave;
public class RequirementUI : MonoBehaviour{
	[SerializeField] private Text description;
	public Requirement requirement;
	public Button buttonGetReward;
	public  ItemSliderControllerScript sliderAmount;
	public RewardUIControllerScript rewardController;
	public void ChangeProgress(BigDigit amount){
		if(requirement.CurrentStage < requirement.CountStage){
			requirement.AddProgress(amount);
			UpdateUI();
                  OnChange();
		}
	}
	
	public void GetReward(){
		requirement.GetReward();
		UpdateUI();
            OnChange();
	}
	public void SetData(Requirement requirement){
		this.requirement = requirement;
		description.text = requirement.description;
		UpdateUI();
		if(requirement.CurrentStage < requirement.CountStage ){
			SubscribeAction();
		}
	}
      public void SetProgress(RequirementSave requirementSave){
            requirement.SetProgress(requirementSave.currentStage, requirementSave.progress);
            UpdateUI();
      }
	public void UpdateUI(){
		rewardController.ShowReward(requirement.GetRewardInfo());
		if(requirement.CurrentStage < requirement.CountStage){
			buttonGetReward.interactable = requirement.CheckCount();
			sliderAmount.SetAmount(requirement.Progress, requirement.GetRequireCount());
                  buttonGetReward.gameObject.SetActive(true);
		}else{
			buttonGetReward.gameObject.SetActive(false);
			sliderAmount.Hide();
		}
	}
	private void SubscribeAction(){
		switch(requirement.type){
            case TypeRequirement.SimpleSpin:
            	WheelFortuneScript.Instance.RegisterOnSimpleRotate(ChangeProgress);
            	break;
            case TypeRequirement.SpecialHireCount:
            	TavernScript.Instance.RegisterOnSpecialHire(ChangeProgress);
            	break;
            case TypeRequirement.GetLevel:
            	PlayerScript.Instance.RegisterOnLevelUP(ChangeProgress);
            	break;
            case TypeRequirement.SynthesItem:
            	ForgeScript.Instance.RegisterOnCraft(ChangeProgress);
            	break;	
            case TypeRequirement.DoneChapter:
            case TypeRequirement.DoneMission:
            case TypeRequirement.SpecialSpin:
            case TypeRequirement.BuyItemCount:
            case TypeRequirement.DestroyHeroCount:
            case TypeRequirement.CountWin:
            case TypeRequirement.CountDefeat:
            case TypeRequirement.CountPointsOnSimpleArena:
            case TypeRequirement.CountPointsOnTournament:
                break;
            case TypeRequirement.TryCompleteChallangeTower:
                  ChallengeTowerScript.Instance.RegisterOnTryCompleteMision(ChangeProgress);
                  break;
            case TypeRequirement.CompleteChallengeTower:
                  ChallengeTowerScript.Instance.RegisterOnCompleteMision(ChangeProgress);
                  break;            
            // case TypeRequirement.GetHeroes:
            //     requirementScript.listRequirement[i].requireInt = EditorGUILayout.IntField("Count:", requirementScript.listRequirement[i].requireInt);
            //     break;
            // case TypeRequirement.GetHeroesWithLevel:
            //     requirementScript.listRequirement[i].requireInt = EditorGUILayout.IntField("Count:", requirementScript.listRequirement[i].requireInt);
            //     break;
            // case TypeRequirement.GetHeroesWithRating:
            //     requirementScript.listRequirement[i].requireInt = EditorGUILayout.IntField("Count:", requirementScript.listRequirement[i].requireInt);
            //     break;
            // case TypeRequirement.GetHeroesCount:
            //     requirementScript.listRequirement[i].requireInt = EditorGUILayout.IntField("Count:", requirementScript.listRequirement[i].requireInt);
            //     break;
            // case TypeRequirement.SynthesCount:
            //     requirementScript.listRequirement[i].requireInt = EditorGUILayout.IntField("Count:", requirementScript.listRequirement[i].requireInt);
            //     break;
            // case TypeRequirement.SynthesItem:
            //     requirementScript.listRequirement[i].requireInt = EditorGUILayout.IntField("Count:", requirementScript.listRequirement[i].requireInt);
            //     break;  
            // case TypeRequirement.BuyItem:
            //     requirementScript.listRequirement[i].requireObject = (ScriptableObject) EditorGUILayout.ObjectField ("Object:", requirementScript.listRequirement[i].requireObject, typeof (ScriptableObject), false);
            //     break;
            // case TypeRequirement.SpendResource:
            //     requirementScript.requireRes = (Resource) EditorGUILayout.ObjectField ("Resource:", requirementScript.requireRes, typeof(Resource), true);
            //  break;  
		}
	}
      public void Restart(){
            requirement.ClearProgress();
            UpdateUI();
      }
      private Action observerOnChange;
      public void RegisterOnChange(Action d){observerOnChange += d;}
      public void UnRegisterOnChange(Action d){observerOnChange += d;}
      private void OnChange(){Debug.Log(observerOnChange == null); if(observerOnChange != null) observerOnChange(); Debug.Log("OnChange");}
}