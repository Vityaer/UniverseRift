﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;


	[System.Serializable]
	public class BreakthroughHero{
		public int currentBreakthrough = 0;
		public List<Breakthrough> listBreakthrough =  new List<Breakthrough>();

		public bool OnLevelUp(int level){
			bool result = false;
			if((currentBreakthrough + 1) < listBreakthrough.Count){
				if(listBreakthrough[currentBreakthrough + 1].requireLevel == level){
					if(listBreakthrough[currentBreakthrough + 1].heroes.Count == 0){
						currentBreakthrough++;
						result = true;
					}
				}
			} 
			return result;
		}
		public int LimitLevel{ get => (int) listBreakthrough[currentBreakthrough].newLimitLevel;}
		
		public IncreaseCharacteristics GetGrowth(){
			return listBreakthrough[currentBreakthrough].incCharacts;
		}
		public void ChangeData(GeneralInfoHero generalInfo){
			Breakthrough evolve = listBreakthrough[currentBreakthrough];
			if(evolve.isSeriousChange){
				if(evolve.newName.Length > 0)   generalInfo.Name      = evolve.newName;
				if(evolve.IDnewPrefab != 0)     generalInfo.idHero    = evolve.IDnewPrefab;
				generalInfo.race      = evolve.newRace;
				generalInfo.ClassHero = evolve.newClassHero;
			}
		}
	}
	[System.Serializable]
	public class Breakthrough{
		[Header("Data")]
		public uint numBreakthrough;

		[Header("Require")]
		public uint requireLevel;
		public List<RequireHero> heroes;
		
		[Header("Reward")]
		public uint newLimitLevel;
		public uint newRatingHero = 0;
		public IncreaseCharacteristics incCharacts;

		[Header("Serious changes")]
		public bool isSeriousChange = false;
		public string newName = "";
		public Race newRace;
		public Vocation newClassHero;
		public int IDnewPrefab;

	}
	[System.Serializable]
	public class RequireHero{
		public InfoHero hero; 
		public Race race;
		public uint rating;
		public uint RequireLevel = 1;
		public uint amountHero = 1;
	}
