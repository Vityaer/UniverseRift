using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
public class CardScript : MonoBehaviour
{
	public InfoHero hero;
	[SerializeField] private Image imageUI;
	[SerializeField] private Text  levelUI;
	[SerializeField] private Image panelSelect;
	[SerializeField] private VocationUIScript vocationUI;
	[SerializeField] private RaceUIScript raceUI;
	public RatingHeroScript ratingController;
	private ListCardOnWarTableScript listCardController;
	public bool selected = false;

	void Start()
	{
		UpdateUI();
	}

	private void UpdateUI()
	{
		if(hero != null)
		{
			imageUI.sprite       = hero.generalInfo.ImageHero;
			levelUI.text         = hero.generalInfo.Level.ToString();
			ratingController.ShowRating(hero.generalInfo.ratingHero); 
		}
		else
		{
			listCardController?.RemoveCards(new List<CardScript>(){this});
		}
	}

	private void SetImage(InfoHero data)
	{
		imageUI.sprite = data.generalInfo.ImageHero;
	}

//API
	public void ClickOnCard()
	{
		if(selected == false)
		{
			listCardController.SelectCard(this);
		}
		else
		{
			listCardController.UnSelectCard(this);
		}
	}

	public void Selected()
	{
		selected = true;
		panelSelect.enabled = true;
	}

	public void UnSelected()
	{
		selected = false;
		panelSelect.enabled = false;
	} 

	public void Clear()
	{
		imageUI.sprite = null;
		levelUI.text = string.Empty;
		ratingController.Hide();
		gameObject.SetActive(false);
	}

	public void SetData(RequirementHero requirementHero)
	{
		gameObject.SetActive(true);
		levelUI.text = string.Empty;
		ratingController.ShowRating(requirementHero.rating); 
		// vocationUI.SetData(requirementHero.);
		// raceUI.SetData(requirementHero.);
		SetImage(requirementHero.GetData);
	}

	public void ChangeInfo(InfoHero hero, ListCardOnWarTableScript listCardController)
	{
		this.hero = hero;
		this.listCardController = listCardController;
		UpdateUI();
	}

	public void ChangeInfo(InfoHero hero)
	{
		this.hero = hero;
		UpdateUI();
	}

	public void DestroyCard()
	{
		listCardController.RemoveCardFromList(this);
		Destroy(gameObject);
	}
}
