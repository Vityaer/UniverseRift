using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;
using ObjectSave;
public class PanelPlayerScript : Building{
	[SerializeField] private ItemSliderControllerScript sliderLevel;
	[SerializeField] private TextMeshProUGUI nameText, levelText, IDGuildText, idText;
	[SerializeField] private CostLevelUp playerLevelList, rewardForLevelUp;
	[SerializeField] private Image avatar, outlineAvatar;
	private PlayerInfo playerInfo;
	private Resource requireExpForLevel, currentExp;
	protected override void OnLoadGame(){
		playerInfo = PlayerScript.Instance.player.GetPlayerInfo;
		PlayerScript.Instance.RegisterOnChangeResource(ChangeExp, TypeResource.Exp);
		requireExpForLevel = GetRequireExpForLevel(); 
		currentExp = PlayerScript.Instance.GetResource(TypeResource.Exp);
	} 
	protected override void OpenPage(){
		UpdateMainUI();
	}
	private void UpdateMainUI(){
		nameText.text = playerInfo.Name;
		levelText.text = playerInfo.Level.ToString();
		IDGuildText.text = playerInfo.IDGuild.ToString();
		// avatar.sprite = playerInfo.avatar;
		sliderLevel.SetAmount(currentExp, requireExpForLevel);
	}
//Exp
	public void ChangeExp(Resource newExp){
		if(newExp.CheckCount(requireExpForLevel)){
			PlayerScript.Instance.UnRegisterOnChangeResource(ChangeExp, TypeResource.Exp);
			PlayerScript.Instance.SubtractResource(requireExpForLevel);
			PlayerScript.Instance.player.LevelUP();
			requireExpForLevel = GetRequireExpForLevel();
			Reward reward = new Reward(rewardForLevelUp.GetCostForLevelUp( playerInfo.Level ));
			MessageControllerScript.Instance.OpenPanelNewLevel(reward);
			PlayerScript.Instance.RegisterOnChangeResource(ChangeExp, TypeResource.Exp);
			UpdateMainUI();
		}
	}
	private Resource GetRequireExpForLevel(){ return playerLevelList.GetCostForLevelUp(playerInfo.Level + 1).GetResource(TypeResource.Exp);}

	public void SaveNewName(string newName){
		playerInfo.SetNewName(newName);
		SaveGame();
	}
	public string GetName{get => playerInfo.Name;}
}