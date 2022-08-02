using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
public class NewHeroScript : MonoBehaviour{

	public List<InfoHero> listNewHero = new List<InfoHero>();

	public Color colorNotInteractable;
	public Color colorInteractable;
	public Resource DiamondCost;
	private Button btnBuy;
	private Image sprite; 
	void Awake(){
		btnBuy = GetComponent<Button>();
		sprite = GetComponent<Image>();
	}
	void Start(){
		CheckResourceForBuyHero();
	}
	public void GetNewHero(){
		PlayerScript.Instance.SubtractResource(DiamondCost);
		
		InfoHero hero = new InfoHero(listNewHero[ Random.Range(0, listNewHero.Count) ]);
		hero.generalInfo.Name = hero.generalInfo.Name + " №" + Random.Range(0, 1000).ToString();
		GetNewHero(hero);
		MessageControllerScript.Instance.AddMessage("Новый герой! Это - " + hero.generalInfo.Name);
		CheckResourceForBuyHero();
	}
	public void GetNewHero(InfoHero newHero){
		PlayerScript.Instance.AddHero(newHero);
	}
   	private void CheckResourceForBuyHero(){
		bool result = PlayerScript.Instance.CheckResource(DiamondCost);
		btnBuy.interactable = result;
		sprite.color = (result) ? colorInteractable : colorNotInteractable;
	}
}
