using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;
public class MagicCircleScript : Building{
	private List<InfoHero> listHeroes = new List<InfoHero>();
	protected override void OpenPage(){
		if(listHeroes.Count == 0){
			listHeroes = Tavern.Instance.GetListHeroes;
		}
	}
	[SerializeField] private Race selectedRace;
	private Resource RaceHireCost = new Resource(TypeResource.RaceHireCard, 1, 0);
	public void ChangeHireRace(string stringRace){
		selectedRace = (Race) Enum.Parse(typeof(Race), stringRace);
	}
	private void HireHero(int count = 1){
		float rand = 0f;
		InfoHero hero = null;
		List<InfoHero> workList = new List<InfoHero>();
		for(int i = 0; i < count; i++){
			rand = UnityEngine.Random.Range(0f, 100f);
			if(rand < 96f){
				workList = workList.FindAll(x => ((x.generalInfo.RatingHero == 4) && (x.generalInfo.Race == selectedRace)));
			}else{
				workList = workList.FindAll(x => ((x.generalInfo.RatingHero == 5) && (x.generalInfo.Race == selectedRace)));
			}
			hero = (InfoHero) workList[ UnityEngine.Random.Range(0, workList.Count) ].Clone();

			if(hero != null){
				hero.generalInfo.Name = hero.generalInfo.Name + " №" + UnityEngine.Random.Range(0, 1000).ToString();
				AddNewHero(hero);
			}
		}
	}
	private void AddNewHero(InfoHero hero){
		Tavern.Instance.AddNewHero(hero);
	}
	public ButtonWithObserverResource oneHire, tenHire;
	protected override void OnStart(){
		oneHire.ChangeCost(HireHero);
		tenHire.ChangeCost(HireHero);
	}
}