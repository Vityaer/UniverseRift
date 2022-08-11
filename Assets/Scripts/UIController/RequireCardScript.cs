using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;
public class RequireCardScript : MonoBehaviour{
	[SerializeField] private CardScript card;
	[SerializeField] private TextMeshProUGUI textCountRequirement;
	private int requireSelectCount = 0;
	[Header("Panel select heroes")]
	public OpenClosePanel panelListHeroes;
	public ListCardOnWarTableScript listCard;
	public RequireCardScript requireCardInfo;
	RequirementHero requirementHero;
	public void SetData(RequirementHero requirementHero){
		ClearData();
		this.requirementHero = requirementHero;
		requireSelectCount = requirementHero.count;
		Debug.Log("requirementHero");
		card.SetData(requirementHero);
		UpdateUI();
	}
	List<InfoHero> selectedHeroes = new List<InfoHero>();
	public void AddHero(CardScript card){
		if(selectedHeroes.Count < requireSelectCount){
			card.Selected();
			selectedHeroes.Add(card.hero);
			UpdateUI();
			requireCardInfo.UpdateUI();
		}
	}
	public void RemoveHero(CardScript card){
		if(selectedHeroes.Count > 0){
			card.UnSelected();
			selectedHeroes.Remove(card.hero);
			UpdateUI();
			requireCardInfo.UpdateUI();
		}
	}
	public void UpdateUI(){
		textCountRequirement.text = string.Concat(
			selectedHeroes.Count.ToString(),
			"/",
			requireSelectCount.ToString()
		);
	}
	public bool CheckHeroes(){
		bool result = false;
		return result;
	}
	public void OpenListCard(){
		listCard.RegisterOnSelect(AddHero);
		listCard.RegisterOnUnSelect(RemoveHero);
		List<InfoHero> currentHeroes = PlayerScript.Instance.GetListHeroes;
		currentHeroes = currentHeroes.FindAll(x => x.CheckСonformity(requirementHero) );
		currentHeroes.Remove(TrainCampScript.Instance.ReturnSelectHero());
		listCard.SetList(currentHeroes);
		listCard.SelectCards(selectedHeroes);
		panelListHeroes.Open();
		requireCardInfo.ShowData(this.requirementHero, selectedHeroes);
		panelListHeroes.RegisterOnClose(OnClosePanelHeroes);
	}
	void OnClosePanelHeroes(){
		panelListHeroes.UnregisterOnClose(OnClosePanelHeroes);
		listCard.UnRegisterOnSelect(AddHero);
		listCard.UnRegisterOnUnSelect(RemoveHero);
	}
	public void ClearData(){
		selectedHeroes.Clear();
	}
	public void Hide(){
		gameObject.SetActive(false);
	}
	public void DeleteSelectedHeroes(){
		for(int i = 0; i < selectedHeroes.Count; i++){
			PlayerScript.Instance.RemoveHero(selectedHeroes[i]);
		}
		ClearData();
	}

	public void ShowData(RequirementHero requirementHero,List<InfoHero> selectedHeroes){
		this.selectedHeroes = selectedHeroes;
		this.requirementHero = requirementHero;
		requireSelectCount = requirementHero.count;
		card.SetData(requirementHero);
		UpdateUI();
	}

}