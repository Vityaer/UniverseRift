using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using System;
using TMPro;
public class WarriorPlaceScript : MonoBehaviour{
    public int ID;
    public  CardScript card;
    private  InfoHero hero;
    public WarTableControllerScript WarTable;
    [SerializeField] private Image ImageHero;
    [SerializeField] private TextMeshProUGUI textLevel;
    public InfoHero Hero => hero;
	void Start(){
        WarTable   = WarTableControllerScript.Instance;
	}
	public void SetHero(CardScript card, InfoHero hero){
		if(card != null) this.card = card;
        this.hero = hero;
		card.Selected();
		UpdateUI();
	}
	public void OnClickPlace(){ if(card != null) WarTable.UnSelectCard(card);}
    public void ClearPlace(){
        if(card != null){
            card.UnSelected();
            card = null;
        }
        hero = null;
        ClearUI();
    }
	public void UpdateUI()
    {
		ImageHero.sprite = hero?.generalInfo.ImageHero;
        ImageHero.enabled = true; 
        textLevel.text = hero.generalInfo.Level.ToString();
	}
	private void ClearUI()
    {
        ImageHero.enabled = false; 
		ImageHero.sprite = null;
        textLevel.text = string.Empty;
	}
    public void SetEnemy(MissionEnemy enemy)
    {
        hero = enemy.enemyPrefab;
        hero.PrepareHeroWithLevel(enemy.level);
        UpdateUI();
    }
    public bool IsEmpty() => (card == null);
}
