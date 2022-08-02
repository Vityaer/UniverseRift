using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;
using UnityEngine.UI;
using ObjectSave;
using TMPro;
public class MineControllerScript : MonoBehaviour{
	[Header("Data")]
	[SerializeField] private int id;
	[SerializeField] private Mine mine;
	[Header("UI")]
	[SerializeField] private Image image;
	public Mine GetMine{get => mine;}
	public int ID{get => id;}
	public Animator anim;
	public void LoadMine(MineSave mineSave){
		mine.SetData(mineSave);
	}
	public void CreateMine(MineSave mineSave){
		// anim?.Play("Create");
		mine.SetData(mineSave);
		PlayerScript.GetPlayerGame.SaveMine(this);
	}
	public void UpdateLevel(){
		mine.LevelUP();
		PlayerScript.GetPlayerGame.SaveMine(this);
	}
	public void GetReward(){
		mine.GetResources();
		PlayerScript.GetPlayerGame.SaveMine(this);
	}
	public void OpenPanelInfo(){
		MinesScript.Instance.panelMineInfo.SetData(this);
	}
}
public enum TypeMine{
	Diamond = 0,
	Gold = 1,
	RedDust = 2,
	Energy = 100,
	Attack = 101,
	HP = 102,
	Main = 1000
}
public enum TypeStore{
	Percent = 0,
	Num = 1
}