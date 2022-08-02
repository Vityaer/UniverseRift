using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class SkillObject : MonoBehaviour{

	public Image image;
	public Skill skill;
	public SkillInfoControllerScript detailController;

	public void SetInfo(Skill skill){
		this.skill = skill;
		image.sprite = this.skill.image;
		gameObject.SetActive(true);
	}
	
	public void OpenDetail(){
		detailController.ShowSkill(skill);
	}
	public void OffObject(){
		skill = null;
		gameObject.SetActive(false);
	}
}
