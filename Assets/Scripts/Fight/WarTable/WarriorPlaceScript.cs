using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using System;
public class WarriorPlaceScript : MonoBehaviour{
    public int ID;
    public  CardScript card;
    public  InfoHero hero;
    public WarTableControllerScript WarTable;
    private Image ImageHero;
    private Text textLevel;
	void Start(){
        ImageHero = transform.Find("ImageHero").GetComponent<Image>();
		textLevel = transform.Find("TextLevel").GetComponent<Text>();
        WarTable   = WarTableControllerScript.Instance;
        if(hero?.generalInfo.Prefab != null) UpdateUI();
	}
	public void SetHero(CardScript card, InfoHero hero){
		if(card != null) this.card = card;
        this.hero = hero;
		card.Selected();
		UpdateUI();
	}
	public void OnClickPlace(){ if(card != null) WarTable.UnSelectCard(card);}
    public void ClearPlace(){
        card.UnSelected();
        card = null;
        hero = null;
        ClearUI();
    }
	public void UpdateUI(){
		ImageHero.sprite = hero?.generalInfo.ImageHero;
        ImageHero.enabled = true; 
        textLevel.text = hero.generalInfo.Level.ToString();
	}
	public void ClearUI(){
        ImageHero.enabled = false; 
		ImageHero.sprite = null;
        textLevel.text = "";
	}
    public void SetEnemy(MissionEnemy enemy){
        hero = new InfoHero(enemy.enemyPrefab);
        hero.PrepareHeroWithLevel(enemy.level);
        hero.characts.HP = Math.Max(enemy.GetHP, hero.characts.HP);
        UpdateUI();
    }
    public bool IsEmpty(){ return (card == null) ? true : false; }
}
