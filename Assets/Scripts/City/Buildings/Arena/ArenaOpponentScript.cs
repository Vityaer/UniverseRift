using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ArenaOpponentScript : MonoBehaviour{
	[SerializeField] private Image avatar;
	[SerializeField] private Text  nameText, levelText, winCountText, loseCountText;
	[SerializeField] private ArenaOpponent opponent;
	void Start(){
		FillInfo();
	}
	private void FillInfo(){
		avatar.sprite = opponent.Avatar;
		nameText.text = opponent.Name;
		levelText.text = string.Concat("Уровень: ", opponent.Level.ToString());
		winCountText.text = opponent.WinCount.ToString();
		loseCountText.text = opponent.LoseCount.ToString();
	}
	public void GoToFight(){
		WarTableControllerScript.Instance.OpenMission(opponent.GetMission(), PlayerScript.Instance.GetListHeroes);
	}  

}
