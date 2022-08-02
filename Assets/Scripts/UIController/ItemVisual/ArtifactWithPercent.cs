using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
public class ArtifactWithPercent : MonoBehaviour{
   	public SubjectCellControllerScript artifactInfo;
	public TextMeshProUGUI textPercent;
	public void SetData(int ID, float percent){
		artifactInfo.SetItem(ArtifactSystem.Instance.GetArtifact(ID));
		textPercent.text = string.Concat(percent.ToString(), "%");
		gameObject.SetActive(true);
	}
	public void Hide(){
   		gameObject.SetActive(false);
   	}
}
