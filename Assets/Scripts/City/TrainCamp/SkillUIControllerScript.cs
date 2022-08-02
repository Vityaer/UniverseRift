using System.Collections;
using System.Collections.Generic;
using UnityEngine;
public class SkillUIControllerScript : MonoBehaviour{
    public List<SkillObject> skillsObject = new List<SkillObject>();

    public void ShowSkills(List<Skill> skills){
    	ClearPanelSkills();
    	for(int i=0; i< skills.Count; i++){
    		skillsObject[i].SetInfo(skills[i]);
    	}
    }
    private void ClearPanelSkills(){
    	foreach (SkillObject obj in skillsObject) {
    		obj.OffObject();
    	}
    }
}
