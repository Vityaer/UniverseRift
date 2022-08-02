using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "LevelUPRaitingHeroes", menuName = "Custom ScriptableObject/LevelUpRatingHeroes", order = 61)]
[System.Serializable]
public class LevelUpRatingHeroes : ScriptableObject{
	[Header("Ratings")]
	public List<LevelUpRaiting> ratings = new List<LevelUpRaiting>();
	public LevelUpRaiting GetRequirements(InfoHero hero){
		int currentRating = hero.generalInfo.ratingHero; 
		LevelUpRaiting result = ratings.Find(x => x.level == (currentRating + 1) );
		if(result != null){
			result.UpdateData(hero);
		}else{
			Debug.Log("not found LevelUpRaiting");
		}
		return result;
	}
}

[System.Serializable]
public class LevelUpRaiting{
    public string Name;
	public int level;
	[Header("Requirements")]
	[SerializeField] private ListResource list;
	public ListResource Cost{get => list;}

	public List<RequirementHero> requirementHeroes = new List<RequirementHero>();

	public void UpdateData(InfoHero hero){ requirementHeroes.ForEach(x => x.UpdateData(hero)); }
}
[System.Serializable]
public class RequirementHero{
	public int ID, rating, count;
	public RequireRaceUpRating requireRace;
	public Race race;
	private InfoHero dataHero;
	public InfoHero GetData{get => dataHero;}
	public void UpdateData(InfoHero hero){
		int IDHero = hero.generalInfo.idHero; 
		race = hero.generalInfo.race;
		dataHero = new InfoHero();
		if(ID == 0){
			ID = IDHero;
			dataHero.generalInfo = (GeneralInfoHero) hero.generalInfo.Clone();
			dataHero.generalInfo.Level = 1;
		}else{
			dataHero.generalInfo = new GeneralInfoHero();
			dataHero.generalInfo.ratingHero = rating;
			dataHero.generalInfo.race = race;
			SpriteName spriteName = SpriteName.OneStarHero;
			switch(rating){
				case 1: 
					spriteName = SpriteName.OneStarHero;
					break;
				case 2: 
					spriteName = SpriteName.TwoStarHero;
					break;
				case 3: 
					spriteName = SpriteName.ThreeStarHero;
					break;
				case 4: 
					spriteName = SpriteName.FourStartHero;
					break;
				case 5: 
					spriteName = SpriteName.FiveStarHero;
					break;				
			}
			if(SystemSprites.Instance == null) Debug.Log("SystemSprites.Instance null");
			if(dataHero == null) Debug.Log("dataHero null");
			if(dataHero.generalInfo == null) Debug.Log("dataHero.generalInfo null");
			dataHero.generalInfo.ImageHero = SystemSprites.Instance.GetSprite(spriteName);
		}
	}	
}
public enum RequireRaceUpRating{
	Equal,
	Friend,
	Enemy,
	God,
	Any
}