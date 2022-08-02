using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
public class SkillInfoControllerScript : MonoBehaviour{

	public GameObject mainPanel;
	public Text textNameSkill;
	public Text textDescriptionSkill;
	public Text textLevelSkill;
	public Image imageSkill;
	public Skill skill;
	public void ShowSkill(Skill skill){
		this.skill = skill;
		textNameSkill.text = skill.Name;
		textDescriptionSkill.text = skill.Description;
		textLevelSkill.text = string.Concat("Уровень ", (skill.Level + 1).ToString());
		imageSkill.sprite = skill.image;
		mainPanel.SetActive(true);
	}

	public void Close(){
		mainPanel.SetActive(false);
	}
}
