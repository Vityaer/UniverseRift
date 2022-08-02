using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "NewSplinter", menuName = "Custom ScriptableObject/Splinter", order = 52)]

[System.Serializable]
public class SplintersList : ScriptableObject{
	[SerializeField]
	private List<Splinter> list = new List<Splinter>();
	public Splinter GetSplinter(int ID){
		Splinter result = null;
		if(ID >= 1000){
			InfoHero hero = TavernScript.Instance.GetInfoHero(ID);
			result = new Splinter(hero);
		}else{
			result = list.Find(x => (x.ID == ID));
		}
		if(result == null){ Debug.Log("Not found splinter with id = " + ID.ToString()); result = list[0];}
		return result;
	}

}
